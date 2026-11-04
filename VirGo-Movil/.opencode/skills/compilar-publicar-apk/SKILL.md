---
name: compilar-publicar-apk
description: Compila, verifica, firma y publica el APK de la app VirGo en GitHub Releases a costo cero. Úsala para preparar o revisar un release; publicar requiere confirmación humana.
---

# Compilar y publicar el APK

> ⚠️ Publicar un release o cambiar los secretos de firma requiere **confirmación explícita** de la persona responsable (Art. XI.5).

## 1. Revisión previa

- [ ] CI en verde en `main`: ktlint, detekt, Lint y pruebas unitarias.
- [ ] `versionCode` incrementado y `versionName` en *SemVer* dentro de `app/build.gradle.kts`.
- [ ] `API_BASE_URL` de *release* apunta a la URL HTTPS vigente del backend (ver `memoria.md`).
- [ ] R8 activo (`isMinifyEnabled = true`) y `network_security_config` de *release* sin tráfico en claro.
- [ ] El keystore y sus contraseñas están solo en GitHub Secrets: `SIGNING_KEYSTORE_BASE64`, `SIGNING_STORE_PASSWORD`, `SIGNING_KEY_ALIAS` y `SIGNING_KEY_PASSWORD`.

## 2. Compilación local (opcional, sin publicar)

```powershell
./gradlew clean ktlintCheck detekt lint testDebugUnitTest pitest assembleDebug
```

## 3. Publicación (camino normal)

1. En *Actions → Release → Run workflow*, elige la rama `main` y escribe la versión `X.Y.Z` (Art. VIII.3). Una fusión a `main` **no** publica nada.
2. El workflow decodifica el keystore, ejecuta `assembleRelease`, verifica la firma con `apksigner verify`, crea la etiqueta `vX.Y.Z` y publica `virgo-vX.Y.Z.apk` en **GitHub Releases** con las notas y el commit.

## 4. Verificación

1. Descarga el APK del release e instálalo en un dispositivo o emulador.
2. Abre la pantalla de diagnóstico (en *debug*) o verifica que la app arranque y muestre el estado de la API.
3. **Prueba de humo:** la app abre sin errores y el diagnóstico llega a la API de producción por HTTPS.
4. Registra en `memoria.md` la versión, el commit, el enlace al release, el resultado y lo aprendido; llena la sección «Release» del `check.md`.

> **Google Play** es el destino final del curso. Requiere una enmienda (Art. VIII.4) por el pago único de la cuenta de desarrollador; mientras tanto, solo GitHub Releases.

## 5. Reversión

Marca el release defectuoso como *pre-release*, publica el anterior como *latest* y registra el motivo en `memoria.md`.
