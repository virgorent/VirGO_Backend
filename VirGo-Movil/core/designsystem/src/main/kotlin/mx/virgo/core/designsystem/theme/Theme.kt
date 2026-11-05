package mx.virgo.core.designsystem.theme

import androidx.compose.foundation.isSystemInDarkTheme
import androidx.compose.material3.MaterialTheme
import androidx.compose.material3.darkColorScheme
import androidx.compose.material3.lightColorScheme
import androidx.compose.runtime.Composable
import androidx.compose.runtime.CompositionLocalProvider

private val LightColorScheme =
    lightColorScheme(
        primary = VirGoPurple,
        onPrimary = CardBackground,
        primaryContainer = VirGoPurpleContainer,
        onPrimaryContainer = VirGoPurpleDark,
        secondary = Slate700,
        onSecondary = CardBackground,
        tertiary = VirGoPurpleDark,
        background = CardBackground,
        onBackground = Slate900,
        surface = CardBackground,
        onSurface = Slate900,
        surfaceVariant = InputBackground,
        onSurfaceVariant = Slate500,
        outline = Slate200,
        outlineVariant = Slate400,
        error = ErrorRed,
        onError = CardBackground,
    )

private val DarkColorScheme =
    darkColorScheme(
        primary = VirGoPurple,
        onPrimary = CardBackground,
        primaryContainer = Slate800,
        onPrimaryContainer = VirGoPurpleLight,
        secondary = Slate200,
        onSecondary = Slate900,
        tertiary = VirGoPurpleLight,
        background = Slate900,
        onBackground = Slate50,
        surface = Slate800,
        onSurface = Slate50,
        surfaceVariant = Slate700,
        onSurfaceVariant = Slate400,
        outline = Slate700,
        outlineVariant = Slate500,
        error = ErrorRed,
        onError = CardBackground,
    )

@Composable
fun VirGoTheme(
    darkTheme: Boolean = isSystemInDarkTheme(),
    content: @Composable () -> Unit,
) {
    val colorScheme = if (darkTheme) DarkColorScheme else LightColorScheme
    val spacing = Spacing()

    CompositionLocalProvider(
        LocalSpacing provides spacing,
    ) {
        MaterialTheme(
            colorScheme = colorScheme,
            typography = VirGoTypography,
            shapes = VirGoShapes,
            content = content,
        )
    }
}
