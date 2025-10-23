#!/bin/bash
echo "Iniciando script de configuração do MongoDB..."

mongo <<EOF
use products-db
db.createCollection("products")