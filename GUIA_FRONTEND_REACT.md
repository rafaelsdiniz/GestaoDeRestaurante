# Guia Frontend React - Sistema de Gestão de Restaurante

Este guia fornece instruções para desenvolver um frontend React que consuma a API REST do Sistema de Gestão de Restaurante.

## 📋 Visão Geral da API

**URL Base:** `http://localhost:5252/api`

**Autenticação:** JWT Bearer Token

**CORS:** Configurado para `http://localhost:5173` (React dev server padrão)

## 🔐 Autenticação e Autorização

### 1. Login
```http
POST /api/auth/login
Content-Type: application/json

{
  "email": "admin@restaurante.com",
  "senha": "senha123"
}
```

**Resposta:**
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "usuario": {
    "id": 1,
    "nome": "Administrador",
    "email": "admin@restaurante.com",
    "tipoUsuario": "Administrador"
  }
}
```

### 2. Cadastro
```http
POST /api/auth/cadastro
Content-Type: application/json

{
  "nome": "Novo Usuário",
  "email": "novo@email.com",
  "senha": "senha123"
}
```

### 3. Uso do Token
Incluir no header de todas as requisições autenticadas:
```
Authorization: Bearer <token>
```

## 👥 Usuários

### Listar Usuários (Admin apenas)
```http
GET /api/usuarios
Authorization: Bearer <token>
```

### Buscar Usuário por ID
```http
GET /api/usuarios/{id}
Authorization: Bearer <token>
```

### Atualizar Usuário
```http
PUT /api/usuarios/{id}
Authorization: Bearer <token>
Content-Type: application/json

{
  "nome": "Nome Atualizado",
  "email": "novoemail@exemplo.com"
}
```

### Deletar Usuário (Admin apenas)
```http
DELETE /api/usuarios/{id}
Authorization: Bearer <token>
```

## 🏠 Endereços

### Listar Endereços do Usuário
```http
GET /api/usuarios/{usuarioId}/enderecos
Authorization: Bearer <token>
```

### Criar Endereço
```http
POST /api/usuarios/{usuarioId}/enderecos
Authorization: Bearer <token>
Content-Type: application/json

{
  "rua": "Rua Exemplo",
  "numero": "123",
  "complemento": "Apto 101",
  "bairro": "Centro",
  "cidade": "São Paulo",
  "estado": "SP",
  "cep": "01001000"
}
```

### Atualizar Endereço
```http
PUT /api/usuarios/{usuarioId}/enderecos/{id}
Authorization: Bearer <token>
Content-Type: application/json

{
  "rua": "Rua Atualizada",
  "numero": "456"
}
```

### Deletar Endereço
```http
DELETE /api/usuarios/{usuarioId}/enderecos/{id}
Authorization: Bearer <token>
```

## 🍽️ Cardápio

### Listar Itens do Cardápio
```http
GET /api/itemcardapio
```

### Buscar Item por ID
```http
GET /api/itemcardapio/{id}
```

### Criar Item (Admin apenas)
```http
POST /api/itemcardapio
Authorization: Bearer <token>
Content-Type: application/json

{
  "nome": "Prato Novo",
  "descricao": "Descrição do prato",
  "precoBase": 29.90,
  "periodo": 1,  // 1 = Almoço, 2 = Jantar
  "ingredientesIds": [1, 2, 3]
}
```

### Atualizar Item (Admin apenas)
```http
PUT /api/itemcardapio/{id}
Authorization: Bearer <token>
Content-Type: application/json

{
  "nome": "Nome Atualizado",
  "precoBase": 34.90
}
```

### Deletar Item (Admin apenas)
```http
DELETE /api/itemcardapio/{id}
Authorization: Bearer <token>
```

## 📋 Pedidos

### Listar Pedidos do Usuário
```http
GET /api/usuarios/{usuarioId}/pedidos
Authorization: Bearer <token>
```

### Criar Pedido
```http
POST /api/usuarios/{usuarioId}/pedidos
Authorization: Bearer <token>
Content-Type: application/json

{
  "periodo": 1,  // 1 = Almoço, 2 = Jantar
  "itensIds": [1, 2, 3],
  "atendimentoId": 1
}
```

### Buscar Pedido por ID
```http
GET /api/usuarios/{usuarioId}/pedidos/{id}
Authorization: Bearer <token>
```

### Cancelar Pedido
```http
DELETE /api/usuarios/{usuarioId}/pedidos/{id}
Authorization: Bearer <token>
```

## 🪑 Reservas

### Listar Reservas do Usuário
```http
GET /api/usuarios/{usuarioId}/reservas
Authorization: Bearer <token>
```

### Criar Reserva
```http
POST /api/usuarios/{usuarioId}/reservas
Authorization: Bearer <token>
Content-Type: application/json

{
  "dataHoraReserva": "2024-04-10T12:00:00",
  "quantidadePessoas": 4,
  "mesaId": 1
}
```

### Buscar Reserva por ID
```http
GET /api/usuarios/{usuarioId}/reservas/{id}
Authorization: Bearer <token>
```

### Cancelar Reserva
```http
PUT /api/usuarios/{usuarioId}/reservas/{id}/cancelar
Authorization: Bearer <token>
```

## 🛠️ Sistema Administrativo

### Dashboard
```http
GET /api/admin/dashboard
Authorization: Bearer <token>
```

**Resposta:**
```json
{
  "totalUsuarios": 50,
  "totalAdministradores": 2,
  "totalClientes": 48,
  "totalPedidos": 120,
  "pedidosHoje": 5,
  "pedidosMes": 45,
  "totalReservas": 80,
  "reservasHoje": 3,
  "reservasMes": 25,
  "totalAtendimentos": 120,
  "atendimentosPresencial": 70,
  "atendimentosDelivery": 30,
  "atendimentosApp": 20,
  "totalItensCardapio": 25,
  "itensAlmoco": 15,
  "itensJantar": 10,
  "faturamentoTotal": 12500.50,
  "faturamentoMes": 3200.75,
  "dataAtualizacao": "2024-04-08T15:30:00"
}
```

### Listar Usuários com Estatísticas
```http
GET /api/admin/usuarios
Authorization: Bearer <token>
```

### Alterar Tipo de Usuário
```http
PUT /api/admin/usuarios/{id}/tipo
Authorization: Bearer <token>
Content-Type: application/json

{
  "novoTipo": "Administrador"
}
```

### Últimos Pedidos
```http
GET /api/admin/pedidos/recentes
Authorization: Bearer <token>
```

### Próximas Reservas
```http
GET /api/admin/reservas/proximas
Authorization: Bearer <token>
```

## 📦 Atendimentos (Admin apenas)

### Listar Atendimentos
```http
GET /api/atendimentos
Authorization: Bearer <token>
```

### Criar Atendimento Presencial
```http
POST /api/atendimentos/presencial
Authorization: Bearer <token>
Content-Type: application/json

{
  "observacao": "Mesa perto da janela"
}
```

### Criar Atendimento Delivery Próprio
```http
POST /api/atendimentos/delivery-proprio
Authorization: Bearer <token>
Content-Type: application/json

{
  "observacaoEntrega": "Deixar na portaria"
}
```

### Criar Atendimento Delivery Aplicativo
```http
POST /api/atendimentos/delivery-aplicativo
Authorization: Bearer <token>
Content-Type: application/json

{
  "nomeAplicativo": "iFood",
  "observacaoEntrega": "Sem talheres"
}
```

## 🧪 Sugestões do Chef

### Listar Sugestões
```http
GET /api/sugestoes-chef
```

### Criar Sugestão (Admin apenas)
```http
POST /api/sugestoes-chef
Authorization: Bearer <token>
Content-Type: application/json

{
  "nomeItem": "Sugestão do Dia",
  "descricao": "Descrição da sugestão",
  "preco": 39.90,
  "periodo": "Jantar"
}
```

### Deletar Sugestão (Admin apenas)
```http
DELETE /api/sugestoes-chef/{id}
Authorization: Bearer <token>
```

## 🧱 Ingredientes (Admin apenas)

### Listar Ingredientes
```http
GET /api/ingredientes
Authorization: Bearer <token>
```

### Criar Ingrediente
```http
POST /api/ingredientes
Authorization: Bearer <token>
Content-Type: application/json

{
  "nome": "Tomate",
  "descricao": "Tomate italiano"
}
```

### Atualizar Ingrediente
```http
PUT /api/ingredientes/{id}
Authorization: Bearer <token>
Content-Type: application/json

{
  "nome": "Tomate Cereja"
}
```

### Deletar Ingrediente
```http
DELETE /api/ingredientes/{id}
Authorization: Bearer <token>
```

## 🪑 Mesas

### Listar Mesas Disponíveis (Público)
```http
GET /api/mesa/disponiveis
```

**Resposta:**
```json
[
  {
    "id": 1,
    "numero": 1,
    "capacidade": 2
  },
  {
    "id": 2,
    "numero": 2,
    "capacidade": 2
  }
]
```

### Listar Todas as Mesas (Admin apenas)
```http
GET /api/mesa
Authorization: Bearer <token>
```

### Criar Mesa (Admin apenas)
```http
POST /api/mesa
Authorization: Bearer <token>
Content-Type: application/json

{
  "numero": 5,
  "capacidade": 4
}
```

### Buscar Mesa por ID (Admin apenas)
```http
GET /api/mesa/{id}
Authorization: Bearer <token>
```

### Atualizar Mesa (Admin apenas)
```http
PUT /api/mesa/{id}
Authorization: Bearer <token>
Content-Type: application/json

{
  "numero": 5,
  "capacidade": 6
}
```

### Deletar Mesa (Admin apenas)
```http
DELETE /api/mesa/{id}
Authorization: Bearer <token>
```

## 🚀 Configuração do React

### 1. Criar projeto React
```bash
npx create-react-app frontend-restaurante
cd frontend-restaurante
```
cd gestao-restaurante-front
npm install
```

### Instalar dependências recomendadas

```bash
npm install axios react-router-dom
npm install -D tailwindcss @tailwindcss/vite
```

- **axios** — requisições HTTP
- **react-router-dom** — navegação entre páginas
- **tailwindcss** — estilização rápida (opcional, pode usar outra lib)

---

## 2. Configurar Tailwind (opcional)

`vite.config.js`:
```js
import { defineConfig } from 'vite'
import react from '@vitejs/plugin-react'
import tailwindcss from '@tailwindcss/vite'

export default defineConfig({
  plugins: [react(), tailwindcss()],
})
```

`src/index.css`:
```css
@import "tailwindcss";
```

---

## 3. Estrutura de pastas sugerida

```
src/
├── api/
│   └── axios.js          ← instância do axios com baseURL e token
├── context/
│   └── AuthContext.jsx   ← estado global de autenticação
├── pages/
│   ├── Login.jsx
│   ├── Cadastro.jsx
│   ├── Cardapio.jsx
│   ├── Reservas.jsx
│   ├── Pedidos.jsx
│   ├── Relatorios.jsx
│   └── Admin/
│       ├── Mesas.jsx
│       ├── Ingredientes.jsx
│       └── SugestoesChefe.jsx
├── components/
│   ├── Navbar.jsx
│   ├── ProtectedRoute.jsx
│   └── ...
├── App.jsx
└── main.jsx
```

---

## 4. Configurar Axios com JWT

`src/api/axios.js`:
```js
import axios from 'axios'

const api = axios.create({
  baseURL: 'https://localhost:7xxx/api', // substitua pela porta real
})

// Injeta o token em toda requisição automaticamente
api.interceptors.request.use((config) => {
  const token = localStorage.getItem('token')
  if (token) config.headers.Authorization = `Bearer ${token}`
  return config
})

export default api
```

---

## 5. Context de autenticação

`src/context/AuthContext.jsx`:
```jsx
import { createContext, useContext, useState } from 'react'

const AuthContext = createContext(null)

export function AuthProvider({ children }) {
  const [usuario, setUsuario] = useState(() => {
    const saved = localStorage.getItem('usuario')
    return saved ? JSON.parse(saved) : null
  })

  function login(dadosLogin) {
    // dadosLogin = { token, nomeUsuario, email, usuarioId }
    localStorage.setItem('token', dadosLogin.token)
    localStorage.setItem('usuario', JSON.stringify(dadosLogin))
    setUsuario(dadosLogin)
  }

  function logout() {
    localStorage.removeItem('token')
    localStorage.removeItem('usuario')
    setUsuario(null)
  }

  return (
    <AuthContext.Provider value={{ usuario, login, logout }}>
      {children}
    </AuthContext.Provider>
  )
}

export const useAuth = () => useContext(AuthContext)
```

`src/main.jsx`:
```jsx
import { StrictMode } from 'react'
import { createRoot } from 'react-dom/client'
import { BrowserRouter } from 'react-router-dom'
import { AuthProvider } from './context/AuthContext'
import App from './App.jsx'
import './index.css'

createRoot(document.getElementById('root')).render(
  <StrictMode>
    <BrowserRouter>
      <AuthProvider>
        <App />
      </AuthProvider>
    </BrowserRouter>
  </StrictMode>
)
```

---

## 6. Rotas (App.jsx)

```jsx
import { Routes, Route, Navigate } from 'react-router-dom'
import { useAuth } from './context/AuthContext'
import Login from './pages/Login'
import Cadastro from './pages/Cadastro'
import Cardapio from './pages/Cardapio'
import Reservas from './pages/Reservas'
import Pedidos from './pages/Pedidos'
import Relatorios from './pages/Relatorios'
import Navbar from './components/Navbar'

function ProtectedRoute({ children }) {
  const { usuario } = useAuth()
  return usuario ? children : <Navigate to="/login" />
}

export default function App() {
  return (
    <>
      <Navbar />
      <Routes>
        <Route path="/login"    element={<Login />} />
        <Route path="/cadastro" element={<Cadastro />} />
        <Route path="/cardapio" element={<Cardapio />} />

        {/* Rotas protegidas */}
        <Route path="/reservas"   element={<ProtectedRoute><Reservas /></ProtectedRoute>} />
        <Route path="/pedidos"    element={<ProtectedRoute><Pedidos /></ProtectedRoute>} />
        <Route path="/relatorios" element={<ProtectedRoute><Relatorios /></ProtectedRoute>} />

        <Route path="*" element={<Navigate to="/cardapio" />} />
      </Routes>
    </>
  )
}
```

---

## 7. Páginas — exemplos de implementação

### 7.1 Login

**Endpoint:** `POST /api/auth/login`
**Body:** `{ email, senha }`
**Retorno:** `{ token, nomeUsuario, email, usuarioId }`

```jsx
// src/pages/Login.jsx
import { useState } from 'react'
import { useNavigate } from 'react-router-dom'
import { useAuth } from '../context/AuthContext'
import api from '../api/axios'

export default function Login() {
  const [form, setForm] = useState({ email: '', senha: '' })
  const [erro, setErro] = useState('')
  const { login } = useAuth()
  const navigate = useNavigate()

  async function handleSubmit(e) {
    e.preventDefault()
    try {
      const { data } = await api.post('/auth/login', form)
      login(data)
      navigate('/cardapio')
    } catch {
      setErro('Email ou senha inválidos.')
    }
  }

  return (
    <form onSubmit={handleSubmit}>
      <h1>Login</h1>
      {erro && <p style={{ color: 'red' }}>{erro}</p>}
      <input
        type="email"
        placeholder="Email"
        value={form.email}
        onChange={e => setForm({ ...form, email: e.target.value })}
        required
      />
      <input
        type="password"
        placeholder="Senha"
        value={form.senha}
        onChange={e => setForm({ ...form, senha: e.target.value })}
        required
      />
      <button type="submit">Entrar</button>
    </form>
  )
}
```

---

### 7.2 Cardápio

**Endpoints:**
- `GET /api/itemcardapio` → lista todos os itens
- `POST /api/itemcardapio` → cria item (body: `{ nome, descricao, precoBase, periodo, ingredientesIds }`)
- `PUT /api/itemcardapio/{id}` → atualiza
- `DELETE /api/itemcardapio/{id}` → remove

**Campos importantes no retorno:**
```json
{
  "id": 1,
  "nome": "Frango Grelhado",
  "descricao": "...",
  "precoBase": 32.90,
  "periodo": 1,
  "ehSugestaoDoChefe": true,
  "ingredientes": ["Frango", "Arroz", "Feijão"]
}
```

**Dica:** `periodo === 1` = Almoço, `periodo === 2` = Jantar. Mostre como badge colorido.

```jsx
// Exemplo: exibir badge de período
const periodoBadge = (periodo) =>
  periodo === 1
    ? <span className="badge almoco">Almoço</span>
    : <span className="badge jantar">Jantar</span>
```

**Exemplo completo de componente Cardápio:**
```jsx
// src/pages/Cardapio.jsx
import { useState, useEffect } from 'react'
import api from '../api/axios'

export default function Cardapio() {
  const [itens, setItens] = useState([])
  const [loading, setLoading] = useState(true)
  const [filtroPeriodo, setFiltroPeriodo] = useState('todos') // 'todos', 'almoco', 'jantar'

  useEffect(() => {
    carregarCardapio()
  }, [])

  async function carregarCardapio() {
    try {
      const { data } = await api.get('/itemcardapio')
      setItens(data)
    } catch (error) {
      console.error('Erro ao carregar cardápio:', error)
    } finally {
      setLoading(false)
    }
  }

  const itensFiltrados = itens.filter(item => {
    if (filtroPeriodo === 'todos') return true
    if (filtroPeriodo === 'almoco') return item.periodo === 1
    if (filtroPeriodo === 'jantar') return item.periodo === 2
    return true
  })

  if (loading) return <div>Carregando cardápio...</div>

  return (
    <div className="p-6">
      <h1 className="text-3xl font-bold mb-6">Cardápio</h1>
      
      <div className="mb-6">
        <label className="mr-4">Filtrar por período:</label>
        <select 
          value={filtroPeriodo}
          onChange={(e) => setFiltroPeriodo(e.target.value)}
          className="border p-2 rounded"
        >
          <option value="todos">Todos</option>
          <option value="almoco">Almoço</option>
          <option value="jantar">Jantar</option>
        </select>
      </div>

      <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
        {itensFiltrados.map(item => (
          <div key={item.id} className="border rounded-lg p-4 shadow hover:shadow-lg transition">
            <div className="flex justify-between items-start mb-2">
              <h2 className="text-xl font-semibold">{item.nome}</h2>
              <span className={`px-2 py-1 rounded text-sm ${
                item.periodo === 1 ? 'bg-yellow-100 text-yellow-800' : 'bg-blue-100 text-blue-800'
              }`}>
                {item.periodo === 1 ? 'Almoço' : 'Jantar'}
              </span>
            </div>
            
            <p className="text-gray-600 mb-3">{item.descricao}</p>
            
            <div className="mb-3">
              <p className="font-bold text-lg">
                {item.precoBase.toLocaleString('pt-BR', { style: 'currency', currency: 'BRL' })}
              </p>
            </div>
            
            <div className="mb-4">
              <p className="text-sm text-gray-500 mb-1">Ingredientes:</p>
              <div className="flex flex-wrap gap-1">
                {item.ingredientes.map((ing, idx) => (
                  <span key={idx} className="bg-gray-100 text-gray-700 px-2 py-1 rounded text-xs">
                    {ing}
                  </span>
                ))}
              </div>
            </div>
            
            {item.ehSugestaoDoChefe && (
              <div className="bg-red-50 border border-red-200 text-red-700 px-3 py-1 rounded text-sm">
                🧑‍🍳 Sugestão do Chef
              </div>
            )}
          </div>
        ))}
      </div>
    </div>
  )
}
```

---

### 7.3 Reservas

**Endpoint:** `POST /api/usuarios/{usuarioId}/reservas`
**Body:** `{ dataHoraReserva, quantidadePessoas, mesaId }`

**Endpoint:** `GET /api/usuarios/{usuarioId}/reservas` → lista reservas do usuário
**Endpoint:** `PUT /api/usuarios/{usuarioId}/reservas/{id}/cancelar` → cancela

**Fluxo sugerido:**
1. Mostrar calendário/datepicker
2. Buscar mesas disponíveis: `GET /api/mesas` → mostrar somente as que cabem a quantidade de pessoas
3. Usuário escolhe mesa e confirma

**Status da reserva (número → texto):**
```js
const statusReserva = {
  1: 'Ativa',
  2: 'Cancelada',
  3: 'Finalizada',
  4: 'Confirmada',
}
```

---

### 7.4 Pedidos

**Fluxo completo:**

**Passo 1 — Criar Atendimento**
`POST /api/atendimento`
```json
// Presencial
{ "tipoAtendimento": 1 }

// Delivery Próprio
{ "tipoAtendimento": 2, "observacaoEntrega": "Entregar no portão" }

// Delivery Aplicativo
{ "tipoAtendimento": 3, "nomeAplicativo": "iFood" }
```
→ Guarda o `id` retornado como `atendimentoId`

**Passo 2 — Criar Pedido**
`POST /api/usuarios/{usuarioId}/pedidos`
```json
{
  "periodo": 1,
  "atendimentoId": 5,
  "itensIds": [1, 2, 3]
}
```

**Retorno do pedido:**
```json
{
  "id": 1,
  "dataHora": "2026-03-30T12:30:00",
  "periodo": 1,
  "subtotal": 91.80,
  "desconto": 0,
  "taxaEntrega": 0,
  "total": 91.80,
  "nomeUsuario": "Carlos Silva",
  "tipoAtendimento": "AtendimentoPresencial",
  "itens": ["Frango Grelhado com Arroz", "Picanha na Chapa"]
}
```

**Dica UI:** Faça um "carrinho" com useState para acumular `itensIds` antes de confirmar.

```jsx
const [carrinho, setCarrinho] = useState([])

function adicionarItem(id) {
  setCarrinho(prev => [...prev, id])
}

function removerItem(id) {
  setCarrinho(prev => {
    const idx = prev.indexOf(id)
    if (idx === -1) return prev
    return [...prev.slice(0, idx), ...prev.slice(idx + 1)]
  })
}
```

---

### 7.5 Relatórios

**Faturamento por tipo de atendimento:**
`GET /api/relatorio/faturamento?dataInicio=2026-03-01&dataFim=2026-03-31`
```json
[
  { "tipoAtendimento": "AtendimentoPresencial", "quantidadePedidos": 5, "totalFaturado": 450.00 },
  { "tipoAtendimento": "DeliveryProprio",        "quantidadePedidos": 3, "totalFaturado": 220.00 },
  { "tipoAtendimento": "DeliveryAplicativo",     "quantidadePedidos": 2, "totalFaturado": 130.00 }
]
```

**Itens mais vendidos:**
`GET /api/relatorio/itens-mais-vendidos`
```json
[
  {
    "itemId": 1,
    "nomeItem": "Frango Grelhado",
    "periodo": "Almoco",
    "quantidadeVendida": 5,
    "totalGerado": 164.50,
    "ehSugestaoChefe": true
  }
]
```

**Dica:** Use um gráfico de barras simples com a lib `recharts`:
```bash
npm install recharts
```

```jsx
import { BarChart, Bar, XAxis, YAxis, Tooltip } from 'recharts'

<BarChart width={500} height={300} data={faturamento}>
  <XAxis dataKey="tipoAtendimento" />
  <YAxis />
  <Tooltip />
  <Bar dataKey="totalFaturado" fill="#8884d8" />
</BarChart>
```

---

## 8. Tabela de todos os endpoints

| Recurso          | Método | URL                                              | Auth? | Admin? |
|------------------|--------|--------------------------------------------------|-------|--------|
| Login            | POST   | `/auth/login`                                    | Não   | Não    |
| Cadastro         | POST   | `/auth/cadastro`                                 | Não   | Não    |
| Listar cardápio  | GET    | `/itemcardapio`                                  | Não   | Não    |
| Criar item       | POST   | `/itemcardapio`                                  | Sim   | Sim    |
| Editar item      | PUT    | `/itemcardapio/{id}`                             | Sim   | Sim    |
| Deletar item     | DELETE | `/itemcardapio/{id}`                             | Sim   | Sim    |
| Listar mesas     | GET    | `/mesa`                                          | Não   | Não    |
| Criar mesa       | POST   | `/mesa`                                          | Sim   | Sim    |
| Listar reservas  | GET    | `/usuarios/{uid}/reservas`                       | Sim   | Não    |
| Criar reserva    | POST   | `/usuarios/{uid}/reservas`                       | Sim   | Não    |
| Cancelar reserva | PUT    | `/usuarios/{uid}/reservas/{id}/cancelar`         | Sim   | Não    |
| Listar endereços | GET    | `/usuarios/{uid}/enderecos`                      | Sim   | Não    |
| Criar endereço   | POST   | `/usuarios/{uid}/enderecos`                      | Sim   | Não    |
| Criar pedido     | POST   | `/usuarios/{uid}/pedidos`                        | Sim   | Não    |
| Listar pedidos   | GET    | `/usuarios/{uid}/pedidos`                        | Sim   | Não    |
| Sugestões chefe  | GET    | `/sugestaochefe`                                 | Não   | Não    |
| Faturamento      | GET    | `/relatorio/faturamento?dataInicio=...&dataFim=..`| Sim  | Sim    |
| Mais vendidos    | GET    | `/relatorio/itens-mais-vendidos`                 | Sim   | Sim    |
| Dashboard admin  | GET    | `/admin/dashboard`                               | Sim   | Sim    |
| Listar usuários  | GET    | `/admin/usuarios`                                | Sim   | Sim    |
| Alterar tipo usuário | PUT | `/admin/usuarios/{id}/tipo`                  | Sim   | Sim    |
| Últimos pedidos  | GET    | `/admin/pedidos/recentes`                        | Sim   | Sim    |
| Próximas reservas| GET    | `/admin/reservas/proximas`                       | Sim   | Sim    |
| Listar atendimentos | GET | `/atendimentos`                              | Sim   | Sim    |
| Criar atendimento| POST   | `/atendimentos/presencial`                       | Sim   | Sim    |
| Criar delivery próprio | POST | `/atendimentos/delivery-proprio`           | Sim   | Sim    |
| Criar delivery app | POST | `/atendimentos/delivery-aplicativo`          | Sim   | Sim    |
| Listar ingredientes | GET | `/ingredientes`                              | Sim   | Sim    |
| Criar ingrediente| POST   | `/ingredientes`                                 | Sim   | Sim    |

---

## 9. Dicas gerais

**Tratar erros de autenticação (401):**
```js
api.interceptors.response.use(
  res => res,
  err => {
    if (err.response?.status === 401) {
      localStorage.clear()
      window.location.href = '/login'
    }
    return Promise.reject(err)
  }
)
```

**Enums para exibição:**
```js
export const PERIODO = { 1: 'Almoço', 2: 'Jantar' }
export const TIPO_ATENDIMENTO = {
  1: 'Presencial',
  2: 'Delivery Próprio',
  3: 'Delivery Aplicativo',
}
export const STATUS_RESERVA = {
  1: 'Ativa',
  2: 'Cancelada',
  3: 'Finalizada',
  4: 'Confirmada',
}
```

**Formatar preço:**
```js
const formatarPreco = (valor) =>
  valor.toLocaleString('pt-BR', { style: 'currency', currency: 'BRL' })
```

**Formatar data:**
```js
const formatarData = (iso) =>
  new Date(iso).toLocaleString('pt-BR', { dateStyle: 'short', timeStyle: 'short' })
```

---

## 10. Testando a API com curl

### Login para obter token
```bash
curl -X POST http://localhost:5252/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{"email":"admin@restaurante.com","senha":"senha123"}'
```

### Listar cardápio (público)
```bash
curl http://localhost:5252/api/itemcardapio
```

### Listar mesas disponíveis (público)
```bash
curl http://localhost:5252/api/mesa/disponiveis
```

### Criar reserva (com token)
```bash
curl -X POST http://localhost:5252/api/usuarios/1/reservas \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer SEU_TOKEN_AQUI" \
  -d '{"dataHoraReserva":"2024-04-10T12:00:00","quantidadePessoas":4,"mesaId":1}'
```

### Dashboard administrativo
```bash
curl http://localhost:5252/api/admin/dashboard \
  -H "Authorization: Bearer SEU_TOKEN_AQUI"
```

### Testar autenticação de admin
```bash
# Tentar acessar endpoint admin sem ser admin (deve retornar 403)
curl http://localhost:5252/api/admin/dashboard \
  -H "Authorization: Bearer TOKEN_DE_CLIENTE"
```

## 11. Ordem sugerida de implementação

1. `Login` e `Cadastro` (auth funcionando, token salvo)
2. `Cardápio` (listagem pública, sem auth)
3. `Reservas` (fluxo com mesas)
4. `Pedidos` (fluxo com carrinho + atendimento)
5. `Relatórios` (gráficos)
6. Páginas de admin (CRUD de mesas, ingredientes, sugestões do chefe)

## 12. Dicas de depuração

### Verificar se API está rodando
```bash
curl -I http://localhost:5252
```

### Verificar logs da aplicação
```bash
# No diretório do projeto C#
dotnet run
```

### Testar conexão com banco de dados
```bash
# Verificar se seed foi aplicado
sqlcmd -S localhost -d GestaoDeRestaurante -Q "SELECT Email, TipoUsuario FROM Usuarios"
```

### Verificar CORS
```bash
# Testar requisição do frontend (porta 5173)
curl -H "Origin: http://localhost:5173" -I http://localhost:5252/api/itemcardapio
```

## 13. Solução de problemas

### Erro 401 (Unauthorized)
- Token expirado ou inválido
- Fazer login novamente
- Verificar se token está sendo enviado no header

### Erro 403 (Forbidden)
- Usuário não tem permissão de administrador
- Verificar se `TipoUsuario = 2` para admin

### Erro 404 (Not Found)
- Endpoint incorreto
- Verificar rota no controller
- Verificar se aplicação está rodando na porta correta

### Erro 500 (Internal Server Error)
- Verificar logs da aplicação
- Verificar conexão com banco de dados
- Verificar migrations aplicadas

## 14. Como obter o usuarioId

O `usuarioId` é retornado no login e deve ser armazenado no frontend:

```json
// Resposta do login
{
  "token": "...",
  "nomeUsuario": "Admin Restaurante",
  "email": "admin@restaurante.com",
  "usuarioId": 1,  // ← Este é o ID que você precisa
  "tipoUsuario": "Administrador"
}
```

**No React, após login:**
```jsx
const { data } = await api.post('/auth/login', { email, senha })
localStorage.setItem('usuarioId', data.usuarioId)
localStorage.setItem('token', data.token)
```

**Usando em requisições:**
```jsx
const usuarioId = localStorage.getItem('usuarioId')
const { data } = await api.get(`/usuarios/${usuarioId}/reservas`)
```

## 15. Recursos úteis

### Usuário admin para testes
- **Email:** admin@restaurante.com
- **Senha:** senha123
- **TipoUsuario:** Administrador (2)
- **usuarioId:** 1

### Usuário cliente para testes
- **Email:** cliente@exemplo.com
- **Senha:** senha123
- **TipoUsuario:** Cliente (0)
- **usuarioId:** 2

### Portas padrão
- **Backend API:** http://localhost:5252
- **Frontend React:** http://localhost:5173
- **Banco de dados:** SQL Server (localhost)

### Endpoints principais
- `GET /api/itemcardapio` - Cardápio público
- `POST /api/auth/login` - Login
- `POST /api/usuarios/{usuarioId}/pedidos` - Criar pedido
- `POST /api/usuarios/{usuarioId}/reservas` - Criar reserva
- `GET /api/admin/dashboard` - Dashboard admin

### Exemplo de fluxo completo

1. **Login:**
```bash
curl -X POST http://localhost:5252/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{"email":"admin@restaurante.com","senha":"senha123"}'
```

2. **Criar reserva (usando usuarioId=1 do login):**
```bash
curl -X POST http://localhost:5252/api/usuarios/1/reservas \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer TOKEN" \
  -d '{"dataHoraReserva":"2024-04-10T12:00:00","quantidadePessoas":4,"mesaId":1}'
```

3. **Verificar dashboard admin:**
```bash
curl http://localhost:5252/api/admin/dashboard \
  -H "Authorization: Bearer TOKEN"
```
