import com.android.build.api.dsl.CommonExtension
import org.gradle.api.artifacts.VersionCatalogsExtension
import org.gradle.kotlin.dsl.configure
import org.gradle.kotlin.dsl.getByType

plugins {
    id("org.jetbrains.kotlin.plugin.compose")
}

configure<CommonExtension> {
    buildFeatures.compose = true
}

val libs = extensions.getByType<VersionCatalogsExtension>().named("libs")

dependencies {
    add("implementation", platform(libs.findLibrary("compose-bom").get()))
    add("implementation", libs.findLibrary("compose-runtime").get())
    add("implementation", libs.findLibrary("compose-ui").get())
    add("implementation", libs.findLibrary("compose-ui-tooling-preview").get())
    add("implementation", libs.findLibrary("compose-material3").get())
    add("implementation", libs.findLibrary("compose-material-icons-extended").get())
    add("implementation", libs.findLibrary("androidx-activity-compose").get())
    add("debugImplementation", libs.findLibrary("compose-ui-tooling").get())
}
