import org.gradle.api.initialization.resolve.RepositoriesMode

pluginManagement {
    repositories {
        google {
            content {
                includeGroupByRegex("com\\.android.*")
                includeGroupByRegex("com\\.google.*")
                includeGroupByRegex("androidx.*")
            }
        }
        mavenCentral()
        gradlePluginPortal()
    }
}

dependencyResolutionManagement {
    repositoriesMode.set(RepositoriesMode.FAIL_ON_PROJECT_REPOS)
    repositories {
        google()
        mavenCentral()
    }
}

rootProject.name = "VirGo-Movil"

includeBuild("build-logic")

include(":app")
include(":core:designsystem")
include(":core:common")
include(":core:domain")
include(":core:network")
include(":core:data")
include(":core:datastore")
include(":core:testing")
