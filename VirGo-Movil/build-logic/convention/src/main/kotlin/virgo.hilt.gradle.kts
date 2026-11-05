import org.gradle.api.artifacts.VersionCatalogsExtension
import org.gradle.kotlin.dsl.getByType

plugins {
    id("com.google.devtools.ksp")
    id("com.google.dagger.hilt.android")
}

val libs = extensions.getByType<VersionCatalogsExtension>().named("libs")

dependencies {
    add("implementation", libs.findLibrary("hilt-android").get())
    add("ksp", libs.findLibrary("hilt-compiler").get())
}
