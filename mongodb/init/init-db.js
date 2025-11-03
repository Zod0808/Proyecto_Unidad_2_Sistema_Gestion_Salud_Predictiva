// Script de inicialización de MongoDB para RespiCare
// Este script se ejecuta automáticamente cuando se crea el contenedor de MongoDB

// Conectar a la base de datos
db = db.getSiblingDB('respicare');

// Crear colecciones
db.createCollection('usuarios');
db.createCollection('medicalhistories');
db.createCollection('symptomreports');
db.createCollection('aianalyses');
db.createCollection('chatconversations');

// Crear índices para usuarios
db.usuarios.createIndex({ "email": 1 }, { unique: true });
db.usuarios.createIndex({ "role": 1 });
db.usuarios.createIndex({ "isActive": 1 });
db.usuarios.createIndex({ "createdAt": -1 });

// Crear índices para historiales médicos
db.medicalhistories.createIndex({ "patientId": 1 });
db.medicalhistories.createIndex({ "doctorId": 1 });
db.medicalhistories.createIndex({ "date": -1 });
db.medicalhistories.createIndex({ "syncStatus": 1 });
db.medicalhistories.createIndex({ "patientId": 1, "date": -1 });
db.medicalhistories.createIndex({ "doctorId": 1, "date": -1 });
db.medicalhistories.createIndex({ 
    "diagnosis": "text", 
    "patientName": "text", 
    "description": "text" 
});

// Crear índices para reportes de síntomas
db.symptomreports.createIndex({ "userId": 1 });
db.symptomreports.createIndex({ "reportedAt": -1 });
db.symptomreports.createIndex({ "status": 1 });
db.symptomreports.createIndex({ "userId": 1, "reportedAt": -1 });
db.symptomreports.createIndex({ "location.latitude": 1, "location.longitude": 1 });

// Crear índices para análisis de IA
db.aianalyses.createIndex({ "medicalHistoryId": 1 });
db.aianalyses.createIndex({ "urgency": 1 });
db.aianalyses.createIndex({ "confidence": -1 });
db.aianalyses.createIndex({ "timestamp": -1 });
db.aianalyses.createIndex({ "urgency": 1, "confidence": -1 });

// Crear índices para conversaciones de chat
db.chatconversations.createIndex({ "userId": 1 });
db.chatconversations.createIndex({ "isActive": 1 });
db.chatconversations.createIndex({ "lastActivity": -1 });
db.chatconversations.createIndex({ "userId": 1, "lastActivity": -1 });

// Insertar usuario administrador de prueba
db.usuarios.insertOne({
    "name": "Administrador RespiCare",
    "email": "admin@respicare.com",
    "password": "5e884898da28047151d0e56f8dc6292773603d0d6aabbdd62a11ef721d1542d8", // Hash SHA256 de "password123"
    "role": "Admin",
    "avatar": null,
    "isActive": true,
    "lastLogin": null,
    "createdAt": new Date(),
    "updatedAt": new Date()
});

// Insertar usuario doctor de prueba
db.usuarios.insertOne({
    "name": "Dr. Juan Pérez",
    "email": "doctor@respicare.com",
    "password": "5e884898da28047151d0e56f8dc6292773603d0d6aabbdd62a11ef721d1542d8", // Hash SHA256 de "password123"
    "role": "Doctor",
    "avatar": null,
    "isActive": true,
    "lastLogin": null,
    "createdAt": new Date(),
    "updatedAt": new Date()
});

// Insertar usuario paciente de prueba
db.usuarios.insertOne({
    "name": "María García",
    "email": "paciente@respicare.com",
    "password": "5e884898da28047151d0e56f8dc6292773603d0d6aabbdd62a11ef721d1542d8", // Hash SHA256 de "password123"
    "role": "Patient",
    "avatar": null,
    "isActive": true,
    "lastLogin": null,
    "createdAt": new Date(),
    "updatedAt": new Date()
});

// Insertar historiales médicos de prueba para el paciente
db.medicalhistories.insertMany([
    {
        "patientId": "1", // ID del paciente de prueba
        "doctorId": "2",  // ID del doctor de prueba
        "patientName": "María García",
        "age": 35,
        "diagnosis": "Asma leve",
        "symptoms": [
            { "name": "Tos seca", "severity": "Moderate", "duration": "3 días", "description": "Tos constante" },
            { "name": "Dificultad respiratoria", "severity": "Mild", "duration": "3 días", "description": "Dificultad al respirar" },
            { "name": "Sibilancias", "severity": "Mild", "duration": "3 días", "description": "Sonidos al respirar" }
        ],
        "description": "Paciente presenta síntomas de asma leve. Se recomienda usar inhalador preventivo y evitar exposición a alérgenos.",
        "date": new Date('2024-01-15'),
        "location": {
            "latitude": -12.0464,
            "longitude": -77.0428,
            "address": "Lima, Perú"
        },
        "images": [],
        "audioNotes": null,
        "isOffline": false,
        "syncStatus": "synced",
        "isSynced": true,
        "needsSync": false,
        "isUrgent": false,
        "hasLocation": true,
        "createdAt": new Date('2024-01-15'),
        "updatedAt": new Date('2024-01-15')
    },
    {
        "patientId": "1",
        "doctorId": "2",
        "patientName": "María García",
        "age": 35,
        "diagnosis": "Bronquitis aguda",
        "symptoms": [
            { "name": "Tos con flemas", "severity": "Severe", "duration": "5 días", "description": "Tos con expectoración" },
            { "name": "Dolor de pecho", "severity": "Moderate", "duration": "5 días", "description": "Dolor intermitente" },
            { "name": "Fiebre baja", "severity": "Mild", "duration": "2 días", "description": "Fiebre de hasta 38°C" }
        ],
        "description": "Bronquitis aguda probablemente viral. Se prescribió reposo, líquidos y medicamentos para la tos.",
        "date": new Date('2024-02-20'),
        "location": {
            "latitude": -12.0464,
            "longitude": -77.0428,
            "address": "Lima, Perú"
        },
        "images": [],
        "audioNotes": null,
        "isOffline": false,
        "syncStatus": "synced",
        "isSynced": true,
        "needsSync": false,
        "isUrgent": false,
        "hasLocation": true,
        "createdAt": new Date('2024-02-20'),
        "updatedAt": new Date('2024-02-20')
    },
    {
        "patientId": "1",
        "doctorId": "2",
        "patientName": "María García",
        "age": 35,
        "diagnosis": "Chequeo preventivo",
        "symptoms": [],
        "description": "Chequeo anual. Paciente en buen estado de salud general. Función pulmonar dentro de parámetros normales.",
        "date": new Date('2024-03-10'),
        "location": {
            "latitude": -12.0464,
            "longitude": -77.0428,
            "address": "Lima, Perú"
        },
        "images": [],
        "audioNotes": null,
        "isOffline": false,
        "syncStatus": "synced",
        "isSynced": true,
        "needsSync": false,
        "isUrgent": false,
        "hasLocation": true,
        "createdAt": new Date('2024-03-10'),
        "updatedAt": new Date('2024-03-10')
    }
]);

// Mensaje de confirmación
print('✅ Base de datos RespiCare inicializada correctamente');
print('✅ Colecciones creadas: usuarios, medicalhistories, symptomreports, aianalyses, chatconversations');
print('✅ Índices creados para optimizar consultas');
print('✅ Usuarios de prueba creados:');
print('   - admin@respicare.com (password123) - Administrador');
print('   - doctor@respicare.com (password123) - Doctor');
print('   - paciente@respicare.com (password123) - Paciente');
print('✅ Historiales médicos de prueba insertados para el paciente');

