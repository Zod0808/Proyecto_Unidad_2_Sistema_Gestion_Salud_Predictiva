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

// Mensaje de confirmación
print('✅ Base de datos RespiCare inicializada correctamente');
print('✅ Colecciones creadas: usuarios, medicalhistories, symptomreports, aianalyses, chatconversations');
print('✅ Índices creados para optimizar consultas');
print('✅ Usuarios de prueba creados:');
print('   - admin@respicare.com (password123) - Administrador');
print('   - doctor@respicare.com (password123) - Doctor');
print('   - paciente@respicare.com (password123) - Paciente');

