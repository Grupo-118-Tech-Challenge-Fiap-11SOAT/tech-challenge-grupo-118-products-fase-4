# 📦 Products API

API responsável pelo cadastro e gerenciamento de produtos, utilizando .NET, MongoDB.

---

## 🚀 Visão Geral

A API trabalha com o conceito de **Produto**, que pode assumir diferentes tipos. Cada tipo herda de um DTO base (`ProductDto`) e é identificado através da propriedade **`type`** no JSON.

Tipos disponíveis:
- `snack`
- `drink`
- `dessert`
- `accompaniment`

---

## 🧱 Estrutura Base do Produto

Todos os produtos compartilham os seguintes campos:

| Campo     | Tipo      | Obrigatório | Descrição |
|----------|----------|-------------|----------|
| id       | ObjectId | Não         | Identificador do produto |
| name     | string   | Sim         | Nome do produto |
| price    | decimal  | Sim         | Preço |Markdown Preview Enhanced
| images   | array    | Não         | Lista de imagens |
| isActive | boolean  | Sim         | Indica se o produto está ativo |
| type     | string   | Sim         | Tipo do produto (discriminador polimórfico) |

---

## 🖼️ Primeiros Passos

Siga o tutorial da [documentação](https://github.com/Grupo-118-Tech-Challenge-Fiap-11SOAT/database-terraform-infra-grupo-118-fase-3) do banco de dado de produto.
Após o terraform ser executado, digite o comando terraform output db_user_password para obter a senhar gerada.
Com o banco de dados de Produtos já criado, acesse o projeto na interface MongoDb Atlas e clique em connect -> Compass -> Copie a string de conexão e substitua a senha.

## Endpoint de Criação de Produto

### 🍔 Snack
Campos específicos:
ingredients (obrigatório)

```json
{
  "type": "snack",
  "name": "Hambúrguer Artesanal",
  "price": 29.90,
  "isActive": true,
  "ingredients": [
    "Pão",
    "Carne",
    "Queijo",
    "Alface"
  ],
  "images": [
    {
      "position": 1,
      "url": "https://example.com/burger.png"
    }
  ]
}
```

### 🥤 Drink

Campos específicos:
size (obrigatório)
flavor (opcional)

```json
{
  "type": "drink",
  "name": "Refrigerante",
  "price": 7.50,
  "isActive": true,
  "size": "500ml",
  "flavor": "Cola",
  "images": [
    {
      "position": 1,
      "url": "https://example.com/drink.png"
    }
  ]
}
```

### 🍰 Dessert

Campos específicos:
portionSize (obrigatório)

```json
{
  "type": "dessert",
  "name": "Cheesecake",
  "price": 15.00,
  "isActive": true,
  "portionSize": "1 fatia",
  "images": [
    {
      "position": 1,
      "url": "https://example.com/cheesecake.png"
    }
  ]
}
```

### 🍟 Accompaniment

Campos específicos:
size (obrigatório)

```json
{
  "type": "accompaniment",
  "name": "Batata Frita",
  "price": 12.00,
  "isActive": true,
  "size": "Média",
  "images": [
    {
      "position": 1,
      "url": "https://example.com/fries.png"
    }
  ]
}
```