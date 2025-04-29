
Gestión Académica - Web API
Esta es una Web API diseñada para gestionar un sistema académico, permitiendo a diferentes usuarios interactuar con las funcionalidades según su rol: Administrador, Usuario y Estudiante. El objetivo es proporcionar un sistema flexible, seguro y eficiente para administrar información académica.

Características principales
Roles de usuario
- Administrador:- Gestión de usuarios (crear, actualizar, eliminar).
- Gestión de cursos, asignaturas y recursos académicos.
- Supervisión de estadísticas del sistema.

- Usuario:- Gestión de datos personales.
- Acceso a listas de asignaturas y eventos.
- Gestión de calificaciones propias de los estudiantes.

- Estudiante:- Visualización de cursos y calificaciones.
- Acceso al calendario académico.
- Descarga de materiales educativos.



Endpoints principales
| Método HTTP | Endpoint | Descripción | Acceso | 
| POST | /auth/login | Iniciar sesión en el sistema | Todos | 
| POST | /auth/register | Registrar un nuevo usuario | Administrador | 
| GET | /courses | Obtener lista de cursos | Administrador, Usuario | 
| POST | /courses | Crear un nuevo curso | Administrador | 
| PUT | /courses/{id} | Editar detalles de un curso | Administrador | 
| DELETE | /courses/{id} | Eliminar un curso | Administrador | 
| GET | /grades/{studentId} | Obtener calificaciones de un estudiante | Usuario, Estudiante | 
| GET | /calendar | Visualizar eventos del calendario académico | Todos | 
| POST | /materials/upload | Subir material educativo | Administrador | 
| GET | /materials/download/{id} | Descargar material educativo | Usuario, Estudiante | 



Tecnologías utilizadas
- Framework de desarrollo: Flask (Python) / Express.js (Node.js).
- Base de datos: PostgreSQL / MongoDB.
- Autenticación: JWT (JSON Web Tokens) para sesiones seguras.
- Documentación API: Swagger UI / Postman.


Instalación y configuración
Requisitos previos
- Tener instalado Python 3.x o Node.js.
- Gestor de bases de datos (PostgreSQL, MongoDB, etc.).

Instalación
- Clona este repositorio:git clone https://github.com/tu_usuario/gestion_academica_api.git
cd gestion_academica_api

- Instala las dependencias:pip install -r requirements.txt    # Para Python
npm install                        # Para Node.js

- Configura las variables de entorno:- Crea un archivo .env y define las variables como:DATABASE_URL=tu_url_de_base_de_datos
SECRET_KEY=tu_clave_secreta_jwt


- Ejecuta el servidor:python app.py       # Para Python
node server.js      # Para Node.js



Ejemplo de Uso
Autenticación
Para obtener un token JWT:
POST /auth/login
Content-Type: application/json
Body: {
    "email": "usuario@example.com",
    "password": "tu_contraseña"
}


Crear un Curso
POST /courses
Headers: {
    "Authorization": "Bearer <tu_token_jwt>"
}
Body: {
    "name": "Matemáticas Avanzadas",
    "description": "Curso de nivel avanzado para estudiantes de ingeniería."
}


