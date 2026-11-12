package mx.virgo.app.ui.login

import androidx.compose.foundation.background
import androidx.compose.foundation.layout.Arrangement
import androidx.compose.foundation.layout.Box
import androidx.compose.foundation.layout.Column
import androidx.compose.foundation.layout.Row
import androidx.compose.foundation.layout.Spacer
import androidx.compose.foundation.layout.fillMaxSize
import androidx.compose.foundation.layout.fillMaxWidth
import androidx.compose.foundation.layout.height
import androidx.compose.foundation.layout.padding
import androidx.compose.foundation.layout.size
import androidx.compose.foundation.layout.width
import androidx.compose.foundation.rememberScrollState
import androidx.compose.foundation.shape.CircleShape
import androidx.compose.foundation.shape.RoundedCornerShape
import androidx.compose.foundation.text.KeyboardOptions
import androidx.compose.foundation.verticalScroll
import androidx.compose.material.icons.Icons
import androidx.compose.material.icons.automirrored.filled.ArrowBack
import androidx.compose.material.icons.filled.Visibility
import androidx.compose.material.icons.filled.VisibilityOff
import androidx.compose.material3.HorizontalDivider
import androidx.compose.material3.Icon
import androidx.compose.material3.IconButton
import androidx.compose.material3.MaterialTheme
import androidx.compose.material3.Surface
import androidx.compose.material3.Text
import androidx.compose.material3.TextButton
import androidx.compose.runtime.Composable
import androidx.compose.runtime.getValue
import androidx.compose.runtime.mutableStateOf
import androidx.compose.runtime.saveable.rememberSaveable
import androidx.compose.runtime.setValue
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.graphics.Brush
import androidx.compose.ui.graphics.Color
import androidx.compose.ui.text.font.FontWeight
import androidx.compose.ui.text.input.ImeAction
import androidx.compose.ui.text.input.KeyboardType
import androidx.compose.ui.text.input.PasswordVisualTransformation
import androidx.compose.ui.text.input.VisualTransformation
import androidx.compose.ui.tooling.preview.Preview
import androidx.compose.ui.unit.dp
import androidx.compose.ui.unit.sp
import mx.virgo.core.designsystem.components.VirGoButton
import mx.virgo.core.designsystem.components.VirGoOutlinedButton
import mx.virgo.core.designsystem.components.VirGoTextField
import mx.virgo.core.designsystem.theme.Slate400
import mx.virgo.core.designsystem.theme.Slate500
import mx.virgo.core.designsystem.theme.Slate800
import mx.virgo.core.designsystem.theme.Slate900
import mx.virgo.core.designsystem.theme.VirGoPurple
import mx.virgo.core.designsystem.theme.VirGoPurpleDark
import mx.virgo.core.designsystem.theme.VirGoTheme

@Composable
fun LoginScreen(
    onLoginClick: (String, String) -> Unit = { _, _ -> },
    onForgotPasswordClick: () -> Unit = {},
    onRegisterClick: () -> Unit = {},
    onBackClick: () -> Unit = {},
) {
    var email by rememberSaveable { mutableStateOf("") }
    var password by rememberSaveable { mutableStateOf("") }
    var isPasswordVisible by rememberSaveable { mutableStateOf(false) }

    Surface(
        modifier = Modifier.fillMaxSize(),
        color = Slate900,
    ) {
        Column(
            modifier = Modifier.fillMaxSize(),
        ) {
            // Header con fondo degradado y textos
            Box(
                modifier =
                    Modifier
                        .fillMaxWidth()
                        .height(280.dp)
                        .background(
                            brush =
                                Brush.verticalGradient(
                                    colors =
                                        listOf(
                                            VirGoPurpleDark,
                                            Slate900,
                                        ),
                                ),
                        ).padding(24.dp),
            ) {
                Column(
                    modifier = Modifier.fillMaxSize(),
                    verticalArrangement = Arrangement.SpaceBetween,
                ) {
                    // Botón circular de regresar
                    IconButton(
                        onClick = onBackClick,
                        modifier =
                            Modifier
                                .size(40.dp)
                                .background(
                                    color = Color.White.copy(alpha = 0.85f),
                                    shape = CircleShape,
                                ),
                    ) {
                        Icon(
                            imageVector = Icons.AutoMirrored.Filled.ArrowBack,
                            contentDescription = "Regresar",
                            tint = Slate900,
                        )
                    }

                    // Títulos del Header
                    Column {
                        Text(
                            text = "VIRGO",
                            style =
                                MaterialTheme.typography.labelMedium.copy(
                                    letterSpacing = 3.sp,
                                    fontWeight = FontWeight.Bold,
                                ),
                            color = Color.White.copy(alpha = 0.9f),
                        )

                        Spacer(modifier = Modifier.height(4.dp))

                        Text(
                            text = "Salones cerca de ti",
                            style = MaterialTheme.typography.headlineLarge,
                            color = Color.White,
                        )
                    }
                }
            }

            // Hoja/Tarjeta principal
            Surface(
                modifier =
                    Modifier
                        .fillMaxSize(),
                shape = RoundedCornerShape(topStart = 32.dp, topEnd = 32.dp),
                color = Color.White,
            ) {
                Column(
                    modifier =
                        Modifier
                            .fillMaxSize()
                            .padding(24.dp)
                            .verticalScroll(rememberScrollState()),
                    verticalArrangement = Arrangement.SpaceBetween,
                ) {
                    Column {
                        Text(
                            text = "Qué gusto verte",
                            style = MaterialTheme.typography.headlineMedium,
                            color = Slate900,
                        )

                        Spacer(modifier = Modifier.height(20.dp))

                        // Botones Sociales (Google y Apple)
                        Row(
                            modifier = Modifier.fillMaxWidth(),
                            horizontalArrangement = Arrangement.spacedBy(12.dp),
                        ) {
                            VirGoOutlinedButton(
                                onClick = { },
                                modifier = Modifier.weight(1f),
                            ) {
                                Text(
                                    text = "Google",
                                    style = MaterialTheme.typography.titleMedium,
                                    color = Slate800,
                                )
                            }

                            VirGoOutlinedButton(
                                onClick = { },
                                modifier = Modifier.weight(1f),
                            ) {
                                Text(
                                    text = "Apple",
                                    style = MaterialTheme.typography.titleMedium,
                                    color = Slate800,
                                )
                            }
                        }

                        Spacer(modifier = Modifier.height(20.dp))

                        // Divisor "o con tu correo"
                        Row(
                            modifier = Modifier.fillMaxWidth(),
                            verticalAlignment = Alignment.CenterVertically,
                        ) {
                            HorizontalDivider(
                                modifier = Modifier.weight(1f),
                                color = MaterialTheme.colorScheme.outline,
                            )
                            Text(
                                text = "o con tu correo",
                                style = MaterialTheme.typography.bodyMedium,
                                color = Slate400,
                                modifier = Modifier.padding(horizontal = 12.dp),
                            )
                            HorizontalDivider(
                                modifier = Modifier.weight(1f),
                                color = MaterialTheme.colorScheme.outline,
                            )
                        }

                        Spacer(modifier = Modifier.height(20.dp))

                        // Inputs del Formulario
                        VirGoTextField(
                            value = email,
                            onValueChange = { email = it },
                            placeholder = "Correo",
                            keyboardOptions =
                                KeyboardOptions(
                                    keyboardType = KeyboardType.Email,
                                    imeAction = ImeAction.Next,
                                ),
                        )

                        Spacer(modifier = Modifier.height(14.dp))

                        VirGoTextField(
                            value = password,
                            onValueChange = { password = it },
                            placeholder = "Contraseña",
                            visualTransformation =
                                if (isPasswordVisible) {
                                    VisualTransformation.None
                                } else {
                                    PasswordVisualTransformation()
                                },
                            keyboardOptions =
                                KeyboardOptions(
                                    keyboardType = KeyboardType.Password,
                                    imeAction = ImeAction.Done,
                                ),
                            trailingIcon = {
                                IconButton(onClick = { isPasswordVisible = !isPasswordVisible }) {
                                    Icon(
                                        imageVector =
                                            if (isPasswordVisible) {
                                                Icons.Default.Visibility
                                            } else {
                                                Icons.Default.VisibilityOff
                                            },
                                        contentDescription =
                                            if (isPasswordVisible) {
                                                "Ocultar contraseña"
                                            } else {
                                                "Mostrar contraseña"
                                            },
                                        tint = Slate400,
                                    )
                                }
                            },
                        )

                        // Enlace ¿Olvidaste tu contraseña?
                        Box(
                            modifier = Modifier.fillMaxWidth(),
                            contentAlignment = Alignment.CenterEnd,
                        ) {
                            TextButton(onClick = onForgotPasswordClick) {
                                Text(
                                    text = "¿Olvidaste tu contraseña?",
                                    style = MaterialTheme.typography.labelMedium,
                                    color = VirGoPurple,
                                )
                            }
                        }

                        Spacer(modifier = Modifier.height(12.dp))

                        // Botón Principal
                        VirGoButton(
                            onClick = { onLoginClick(email, password) },
                            text = "Entrar",
                        )
                    }

                    Spacer(modifier = Modifier.height(24.dp))

                    // Footer Regístrate
                    Row(
                        modifier = Modifier.fillMaxWidth(),
                        horizontalArrangement = Arrangement.Center,
                        verticalAlignment = Alignment.CenterVertically,
                    ) {
                        Text(
                            text = "¿Aún no tienes cuenta?",
                            style = MaterialTheme.typography.bodyMedium,
                            color = Slate500,
                        )
                        Spacer(modifier = Modifier.width(4.dp))
                        TextButton(onClick = onRegisterClick) {
                            Text(
                                text = "Regístrate",
                                style = MaterialTheme.typography.labelLarge,
                                color = VirGoPurple,
                            )
                        }
                    }
                }
            }
        }
    }
}

@Preview(showBackground = true)
@Composable
private fun LoginScreenPreview() {
    VirGoTheme {
        LoginScreen()
    }
}
