plugins {
    id("virgo.jvm.library")
}

dependencies {
    api(libs.kotlinx.coroutines.core)
    api(libs.hilt.core)
    api(libs.javax.inject)
}
