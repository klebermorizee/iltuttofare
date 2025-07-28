
# 🛠️ Projeto iltuttofare

## Descrição
`iltuttofare` é um marketplace de serviços inspirado no GetNinjas, voltado para o mercado italiano. O projeto está sendo desenvolvido com:

- Backend em **.NET 8 + Entity Framework Core**
- Banco de dados **PostgreSQL**
- Padrão **RESTful API**
- Autenticação via **JWT com BCrypt**
- Documentação interativa com **Swagger**

---

## ✅ Funcionalidades já implementadas

### 🔐 Autenticação
- Cadastro de profissional com verificação via **SMS**
- Validação do código enviado
- Login com geração de **JWT**
- Endpoint `/me` protegido com `[Authorize]`

### 👤 Profissional
- Registro inicial
- Atualização de dados pessoais via `PUT /me`
- Atualização de subcategorias via `PUT /me/subcategorias`

### 📂 Categorias e Subcategorias
- Modelagem com relacionamento **Categoria → Subcategoria**
- Endpoint `GET /categorias` com inclusão de subcategorias
- Proteção do endpoint via `[Authorize]`

### 🧱 Banco de Dados
- Migrations geradas e aplicadas com sucesso
- Tabelas criadas:
  - `Profissionais`
  - `Categorias`
  - `Subcategorias`
  - `ProfissionalSubcategorias`

### ⚙️ Configuração Técnica
- Projeto inicial criado via CLI
- Pasta `Data` com `AppDbContext`
- Middleware Swagger habilitado
- Autenticação com JWT e senha com BCrypt
- Cors, HTTPS e Swagger funcionando no navegador

---

## 📌 O que falta desenvolver

### 🔧 Backend - próximos passos
- [ ] Endpoint para listar profissionais por subcategoria
- [ ] Endpoint para filtros: cidade, nota, disponibilidade
- [ ] Endpoint para agendamento de serviço
- [ ] Endpoint para avaliação de profissional
- [ ] Painel admin (listagem e moderação)
- [ ] Upload de documentos e imagens (perfil profissional)

### 📲 Frontend
- [ ] Webapp com Angular ou React
- [ ] Tela de login e cadastro
- [ ] Tela de perfil profissional
- [ ] Página pública de busca de profissionais
- [ ] Página de detalhes do profissional
- [ ] Tela de agendamento

### 📱 App Mobile
- [ ] Aplicativo Flutter (após o backend pronto)
- [ ] Cadastro e login
- [ ] Notificações
- [ ] Busca de profissionais
- [ ] Chat em tempo real (Fase avançada)
