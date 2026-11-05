plugins {
    id("virgo.android.library")
}

android {
    namespace = "mx.virgo.core.data"
}

dependencies {
    implementation(project(":core:network"))
    implementation(project(":core:datastore"))
    implementation(project(":core:domain"))
    implementation(project(":core:common"))
}
