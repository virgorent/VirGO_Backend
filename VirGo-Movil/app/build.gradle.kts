plugins {
    id("virgo.android.application")
    id("virgo.android.compose")
    id("virgo.hilt")
}

android {
    namespace = "mx.virgo.app"

    defaultConfig {
        applicationId = "mx.virgo.app"
        versionCode = 1
        versionName = "0.1.0"
    }
}

dependencies {
    implementation(libs.androidx.activity)

    implementation(project(":core:designsystem"))
    implementation(project(":core:common"))
    implementation(project(":core:domain"))
    implementation(project(":core:network"))
    implementation(project(":core:data"))
    implementation(project(":core:datastore"))

    testImplementation(project(":core:testing"))
}
