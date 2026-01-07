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

Para configurar o banco de dados de **Produtos**, siga os passos abaixo.

### 1️ Provisionamento com Terraform

Siga o tutorial disponível na documentação do banco de dados de Produtos para executar o **Terraform**.

Após a execução do Terraform, uma senha será gerada automaticamente para o usuário do banco.

Para visualizá-la, execute o comando:

```bash
terraform output db_user_password
```

Guarde essa senha, pois ela será usada na string de conexão.

### 2 Obter a String de Conexão no MongoDB Atlas
Com o banco de dados de Produtos já criado:

- Acesse o projeto no MongoDB Atlas
- Clique em Connect
- Selecione Compass
- Copie a connection string fornecida

### 3 Ajustar a String de Conexão
Na string de conexão copiada:

- Substitua o valor da senha pela senha obtida via Terraform
- Certifique-se de que o usuário e o cluster estejam corretos

Exemplo: mongodb+srv://<usuario>:<senha>@<cluster>.mongodb.net/products

Após isso, a string estará pronta para ser utilizada na aplicação.

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