"""Builds a Markdown test report from the .trx and Cobertura files under tests/.

Usage: python tools/test_report.py [tests_dir]
Prints Markdown to stdout; CI appends it to $GITHUB_STEP_SUMMARY.
"""
import glob
import os
import sys
import xml.etree.ElementTree as ET

NS = {"t": "http://microsoft.com/schemas/VisualStudio/TeamTest/2010"}
KINDS = {
    "VirGo.UnitTests": "Unitarias",
    "VirGo.IntegrationTests": "Integración",
    "VirGo.SecurityTests": "Seguridad",
    "VirGo.ContractTests": "Contrato",
    "VirGo.DatabaseTests": "Base de datos",
    "VirGo.PerformanceTests": "Rendimiento",
    "VirGo.ArchitectureTests": "Arquitectura",
}
ICON = {"Passed": "✅", "Failed": "❌", "NotExecuted": "⏭️"}


def project_of(path):
    parts = os.path.normpath(path).split(os.sep)
    return next((p for p in parts if p.startswith("VirGo.") and p.endswith("Tests")), "otros")


def seconds(duration):
    h, m, s = (duration or "0:0:0").split(":")
    return int(h) * 3600 + int(m) * 60 + float(s)


def main():
    root = sys.argv[1] if len(sys.argv) > 1 else "tests"
    results, coverage = {}, {}
    for trx in glob.glob(os.path.join(root, "**", "*.trx"), recursive=True):
        tests = results.setdefault(project_of(trx), {})
        for r in ET.parse(trx).getroot().iterfind(".//t:UnitTestResult", NS):
            message = r.find(".//t:ErrorInfo/t:Message", NS)
            tests[r.get("testName")] = (r.get("outcome"), seconds(r.get("duration")), message.text if message is not None else "")
    for cov in glob.glob(os.path.join(root, "**", "coverage.cobertura.xml"), recursive=True):
        rate = ET.parse(cov).getroot().get("line-rate")
        if rate is not None:
            coverage[project_of(cov)] = max(coverage.get(project_of(cov), 0.0), float(rate) * 100)

    if not results:
        print("## Pruebas\n\nNo se encontraron resultados (.trx).")
        return

    total = sum(len(t) for t in results.values())
    failed = sum(1 for t in results.values() for o, _, _ in t.values() if o == "Failed")
    status = "✅ Todas las pruebas pasaron" if failed == 0 else f"❌ {failed} prueba(s) fallaron"
    out = [f"## Pruebas de la API: {status}", "", f"**{total - failed} de {total}** pruebas aprobadas.", ""]
    out += ["| Tipo | Proyecto | Total | ✅ | ❌ | Tiempo | Cobertura de líneas |", "|---|---|---|---|---|---|---|"]
    order = sorted(results, key=lambda p: list(KINDS).index(p) if p in KINDS else 99)
    for p in order:
        t = results[p]
        ok = sum(1 for o, _, _ in t.values() if o == "Passed")
        bad = sum(1 for o, _, _ in t.values() if o == "Failed")
        secs = sum(d for _, d, _ in t.values())
        cov = f"{coverage[p]:.1f} %" if coverage.get(p) else "—"
        out.append(f"| {KINDS.get(p, p)} | `{p}` | {len(t)} | {ok} | {bad} | {secs:.1f} s | {cov} |")
    out += ["", "La cobertura es el porcentaje de líneas de la API que ejecuta cada proyecto por separado. "
            "Las pruebas de base de datos validan los JSON de `db/schema/` y no ejecutan código .NET.", ""]
    if failed:
        out += ["### Pruebas que fallaron", ""]
        for p in order:
            for name, (o, _, msg) in sorted(results[p].items()):
                if o == "Failed":
                    out.append(f"- ❌ `{name}`: {(msg or '').strip().splitlines()[0][:300] if msg else ''}")
        out.append("")
    out += ["### Detalle de cada prueba", ""]
    for p in order:
        t = results[p]
        out += [f"<details><summary>{KINDS.get(p, p)} ({len(t)})</summary>", "", "| | Prueba | Tiempo |", "|---|---|---|"]
        for name, (o, d, _) in sorted(t.items()):
            out.append(f"| {ICON.get(o, o)} | `{name}` | {d * 1000:.0f} ms |")
        out += ["", "</details>", ""]
    sys.stdout.reconfigure(encoding="utf-8")
    print("\n".join(out))


if __name__ == "__main__":
    main()
