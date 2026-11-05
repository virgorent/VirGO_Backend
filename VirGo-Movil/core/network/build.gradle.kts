plugins {
    id("virgo.android.library")
}

android {
    namespace = "mx.virgo.core.network"
}

dependencies {
    implementation(project(":core:common"))
}
