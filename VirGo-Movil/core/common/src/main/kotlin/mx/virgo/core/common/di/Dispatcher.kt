package mx.virgo.core.common.di

import javax.inject.Qualifier

enum class VirgoDispatcher {
    Default,
    IO,
    Main,
}

@Qualifier
@Retention(AnnotationRetention.RUNTIME)
annotation class Dispatcher(
    val dispatcher: VirgoDispatcher,
)
