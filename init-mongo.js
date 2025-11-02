// init-mongo.js
print("Iniciando configuração do banco products-db...");

db = db.getSiblingDB("products-db");

db.createCollection("products");

db.products.insertOne({
  "_t": ["Product", "Snack"],
  "name": "Hamburguer Clássico",
  "price": 25.90,
  "isActive": true,
  "images": [
    {
      "position": 1,
      "url": "https://meuapp.com/images/hamburguer-classico.jpg"
    },
    {
      "position": 2,
      "url": "https://meuapp.com/images/hamburguer-classico-lateral.png"
    }
  ],
  "createdAt": new Date(),
  "updatedAt": new Date()
});

print("✅ Banco 'products-db' configurado com documento inicial!");
