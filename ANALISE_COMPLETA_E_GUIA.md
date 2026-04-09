# Análise Completa - Sistema de Gestão de Restaurante

## Estado Atual do Projeto

### ✅ **O que já está implementado:**

#### **1. Estrutura do Backend (C# ASP.NET Core)**
- **Models**: Todas as entidades principais implementadas
  - Usuario, Endereco, ItemCardapio, Ingrediente, ItemIngrediente (N-N)
  - Atendimento (classe base) com herança: AtendimentoPresencial, AtendimentoDeliveryProprio, AtendimentoDeliveryAplicativo
  - Pedido, ItemPedido, Reserva, Mesa, SugestaoChefe
- **Controllers**: Todos os endpoints necessários
  - AuthController (login/cadastro)
  - ItemCardapioController, IngredienteController, MesaController
  - PedidoController, ReservaController, AtendimentoController
  - SugestaoChefeController, RelatorioController, UsuarioController
- **Services**: Lógica de negócio implementada
- **DTOs**: Objetos de transferência de dados
- **Migrations**: Banco de dados configurado (SQL Server)

#### **2. Funcionalidades Implementadas**
- **Autenticação JWT**: Login e cadastro funcionando
- **Cardápio**: CRUD completo com 20 itens para almoço e 20 para jantar
- **Reservas**: Sistema de reservas para jantar com validações
- **Pedidos**: Sistema completo com cálculo de preços (descontos, taxas)
- **Atendimentos**: Três tipos (presencial, delivery próprio, delivery por aplicativo)
- **Sugestão do Chefe**: Sistema de desconto de 20% com validação diária
- **Relatórios**: Faturamento por tipo de atendimento e itens mais vendidos

#### **3. Banco de Dados**
- **Seed data**: Usuários (incluindo admin), mesas, ingredientes, cardápio
- **Migrações**: Estrutura completa do banco
- **Compatibilidade**: Scripts para garantir compatibilidade

#### **4. Configurações**
- **CORS**: Configurado para React (localhost:5173)
- **JWT**: Autenticação com tokens
- **Swagger**: Documentação da API disponível

### ❌ **O que FALTA na parte administrativa:**

#### **1. Sistema de Roles/Permissões**
- **Problema**: Não há diferenciação entre usuário comum e administrador
- **Solução necessária**:
  - Adicionar campo `Role` ou `TipoUsuario` na entidade Usuario
  - Criar enum: `Cliente = 1`, `Administrador = 2`
  - Modificar seed para marcar admin@restaurante.com como Administrador

#### **2. Middleware de Autorização**
- **Problema**: Todos os endpoints estão acessíveis a qualquer usuário autenticado
- **Solução necessária**:
  - Criar atributo `[Authorize(Roles = "Administrador")]`
  - Aplicar aos endpoints administrativos

#### **3. Endpoints Administrativos Específicos**
Faltam endpoints para:
- **Gerenciamento de Usuários**: Listar todos usuários, alterar permissões
- **Dashboard Administrativo**: Estatísticas em tempo real
- **Configurações do Sistema**: Alterar taxas, horários de funcionamento

#### **4. Painel Administrativo no Frontend**
- **Problema**: O guia frontend não inclui páginas administrativas completas
- **Solução necessária**: Criar seção `/admin` com:
  - Dashboard com métricas
  - Gerenciamento de cardápio
  - Controle de reservas
  - Visualização de pedidos
  - Configurações do sistema

---

## Guia de Implementação - Parte Administrativa

### **Passo 1: Adicionar Sistema de Roles**

#### **1.1 Modificar Model Usuario**
```csharp
// Enums/TipoUsuario.cs
public enum TipoUsuario
{
    Cliente = 1,
    Administrador = 2
}

// Models/Usuario.cs
public class Usuario : BaseEntity
{
    // ... propriedades existentes ...
    
    public TipoUsuario TipoUsuario { get; set; } = TipoUsuario.Cliente;
}
```

#### **1.2 Atualizar Migração**
```bash
dotnet ef migrations add AdicionarTipoUsuario
dotnet ef database update
```

#### **1.3 Modificar Seed**
Atualizar o seed.sql para definir o admin como administrador:
```sql
UPDATE Usuarios 
SET TipoUsuario = 2  -- Administrador
WHERE Email = 'admin@restaurante.com'
```

### **Passo 2: Criar Middleware de Autorização**

#### **2.1 Criar Policy no Program.cs**
```csharp
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Administrador", policy => 
        policy.RequireClaim(ClaimTypes.Role, "Administrador"));
});
```

#### **2.2 Modificar GerarToken no AuthService**
```csharp
private string GerarToken(Usuario usuario)
{
    var claims = new[]
    {
        new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
        new Claim(ClaimTypes.Email, usuario.Email),
        new Claim(ClaimTypes.Name, usuario.Nome),
        new Claim(ClaimTypes.Role, usuario.TipoUsuario.ToString())
    };
    // ... resto do código ...
}
```

### **Passo 3: Proteger Endpoints Administrativos**

Exemplo para ItemCardapioController:
```csharp
[Authorize(Roles = "Administrador")]
[HttpPost]
public async Task<IActionResult> CriarItem([FromBody] ItemCardapioRequestDTO dto)
{
    // ... código existente ...
}
```

Aplicar a todos os endpoints de:
- POST, PUT, DELETE em ItemCardapioController
- POST, PUT, DELETE em IngredienteController  
- POST, PUT, DELETE em MesaController
- SugestaoChefeController (apenas administrador pode definir sugestão)
- RelatorioController (apenas administrador pode acessar)

### **Passo 4: Criar Endpoints Administrativos Adicionais**

#### **4.1 AdminController.cs**
```csharp
[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Administrador")]
public class AdminController : ControllerBase
{
    private readonly AppDbContext _context;
    
    public AdminController(AppDbContext context)
    {
        _context = context;
    }
    
    // Listar todos usuários
    [HttpGet("usuarios")]
    public async Task<IActionResult> ListarUsuarios()
    {
        var usuarios = await _context.Usuarios
            .Select(u => new {
                u.Id,
                u.Nome,
                u.Email,
                TipoUsuario = u.TipoUsuario.ToString(),
                QuantidadePedidos = u.Pedidos.Count,
                QuantidadeReservas = u.Reservas.Count
            })
            .ToListAsync();
            
        return Ok(usuarios);
    }
    
    // Alterar tipo de usuário
    [HttpPut("usuarios/{id}/tipo")]
    public async Task<IActionResult> AlterarTipoUsuario(int id, [FromBody] AlterarTipoUsuarioDTO dto)
    {
        var usuario = await _context.Usuarios.FindAsync(id);
        if (usuario == null) return NotFound();
        
        usuario.TipoUsuario = dto.TipoUsuario;
        await _context.SaveChangesAsync();
        
        return NoContent();
    }
    
    // Dashboard - estatísticas
    [HttpGet("dashboard")]
    public async Task<IActionResult> Dashboard()
    {
        var hoje = DateTime.Today;
        
        var dados = new
        {
            PedidosHoje = await _context.Pedidos
                .Where(p => p.DataHora.Date == hoje)
                .CountAsync(),
            ReservasHoje = await _context.Reservas
                .Where(r => r.DataHoraReserva.Date == hoje && r.Status == StatusReserva.Ativa)
                .CountAsync(),
            FaturamentoHoje = await _context.Pedidos
                .Where(p => p.DataHora.Date == hoje)
                .SumAsync(p => p.Total),
            UsuariosCadastrados = await _context.Usuarios.CountAsync()
        };
        
        return Ok(dados);
    }
}

public class AlterarTipoUsuarioDTO
{
    public TipoUsuario TipoUsuario { get; set; }
}
```

### **Passo 5: Frontend Administrativo**

#### **5.1 Estrutura de Pastas**
```
src/pages/Admin/
├── Dashboard.jsx
├── Usuarios.jsx
├── CardapioAdmin.jsx
├── ReservasAdmin.jsx
├── PedidosAdmin.jsx
└── Configuracoes.jsx
```

#### **5.2 Componente ProtectedAdminRoute**
```jsx
// components/ProtectedAdminRoute.jsx
import { useAuth } from '../context/AuthContext'
import { Navigate } from 'react-router-dom'
import api from '../api/axios'

export default function ProtectedAdminRoute({ children }) {
  const { usuario } = useAuth()
  
  // Verificar se usuário é admin (precisa decodificar JWT ou ter endpoint)
  const isAdmin = usuario?.email === 'admin@restaurante.com'
  // Ou melhor: ter campo role no token
  
  return isAdmin ? children : <Navigate to="/cardapio" />
}
```

#### **5.3 Página de Dashboard Administrativo**
```jsx
// pages/Admin/Dashboard.jsx
import { useState, useEffect } from 'react'
import api from '../../api/axios'

export default function DashboardAdmin() {
  const [estatisticas, setEstatisticas] = useState(null)
  const [carregando, setCarregando] = useState(true)
  
  useEffect(() => {
    carregarDashboard()
  }, [])
  
  async function carregarDashboard() {
    try {
      const { data } = await api.get('/admin/dashboard')
      setEstatisticas(data)
    } catch (erro) {
      console.error('Erro ao carregar dashboard:', erro)
    } finally {
      setCarregando(false)
    }
  }
  
  if (carregando) return <div>Carregando...</div>
  
  return (
    <div className="dashboard-admin">
      <h1>Dashboard Administrativo</h1>
      
      <div className="cards-estatisticas">
        <div className="card">
          <h3>Pedidos Hoje</h3>
          <p className="valor">{estatisticas.pedidosHoje}</p>
        </div>
        
        <div className="card">
          <h3>Reservas Hoje</h3>
          <p className="valor">{estatisticas.reservasHoje}</p>
        </div>
        
        <div className="card">
          <h3>Faturamento Hoje</h3>
          <p className="valor">
            {estatisticas.faturamentoHoje.toLocaleString('pt-BR', {
              style: 'currency',
              currency: 'BRL'
            })}
          </p>
        </div>
        
        <div className="card">
          <h3>Usuários Cadastrados</h3>
          <p className="valor">{estatisticas.usuariosCadastrados}</p>
        </div>
      </div>
      
      {/* Adicionar gráficos, últimas reservas, últimos pedidos */}
    </div>
  )
}
```

#### **5.4 Rotas Administrativas no App.jsx**
```jsx
// Adicionar no App.jsx
<Route path="/admin" element={<ProtectedAdminRoute><AdminLayout /></ProtectedAdminRoute>}>
  <Route index element={<DashboardAdmin />} />
  <Route path="usuarios" element={<UsuariosAdmin />} />
  <Route path="cardapio" element={<CardapioAdmin />} />
  <Route path="reservas" element={<ReservasAdmin />} />
  <Route path="pedidos" element={<PedidosAdmin />} />
  <Route path="configuracoes" element={<ConfiguracoesAdmin />} />
</Route>
```

---

## Endpoints da API (Resumo)

### **Públicos (sem autenticação)**
- `POST /api/auth/login` - Login
- `POST /api/auth/cadastro` - Cadastro
- `GET /api/itemcardapio` - Listar cardápio
- `GET /api/mesa` - Listar mesas
- `GET /api/sugestaochefe` - Sugestões do dia

### **Cliente (autenticação necessária)**
- `GET /api/usuarios/{id}/reservas` - Minhas reservas
- `POST /api/usuarios/{id}/reservas` - Criar reserva
- `PUT /api/usuarios/{id}/reservas/{id}/cancelar` - Cancelar reserva
- `GET /api/usuarios/{id}/pedidos` - Meus pedidos
- `POST /api/usuarios/{id}/pedidos` - Criar pedido
- `POST /api/atendimento` - Criar atendimento

### **Administrador (role Administrador necessária)**
- `POST/PUT/DELETE /api/itemcardapio` - Gerenciar cardápio
- `POST/PUT/DELETE /api/ingrediente` - Gerenciar ingredientes
- `POST/PUT/DELETE /api/mesa` - Gerenciar mesas
- `POST /api/sugestaochefe` - Definir sugestão do chefe
- `GET /api/relatorio/*` - Relatórios
- `GET /api/admin/usuarios` - Listar usuários
- `PUT /api/admin/usuarios/{id}/tipo` - Alterar tipo de usuário
- `GET /api/admin/dashboard` - Dashboard administrativo

---

## Regras de Negócio Implementadas

### ✅ **Já implementadas:**
1. **Cardápio fixo**: 20 itens almoço + 20 itens jantar
2. **Sugestão do Chefe**: Apenas 1 item por período por dia, 20% desconto
3. **Atendimentos**: 3 tipos com herança (presencial, delivery próprio, delivery aplicativo)
4. **Taxas de entrega**: 
   - Delivery próprio: taxa fixa configurável
   - Delivery aplicativo: 4% dia, 6% noite
5. **Reservas**: Apenas para jantar, com validação de horário
6. **Validação de período**: Pedidos de almoço só itens almoço, jantar só itens jantar

### ⚠️ **A verificar/implementar:**
1. **Reserva com antecedência**: "precisa ser agendado com um dia de antecedência" - validar no backend
2. **Horário jantar**: "janela de horário (ex.: 19h–22h)" - configurável no sistema
3. **Código de confirmação**: Opcional para reservas

---

## Próximos Passos Imediatos

### **Prioridade 1 (Crítico para entrega):**
1. **Implementar sistema de roles** (2-3 horas)
2. **Proteger endpoints administrativos** (1-2 horas)
3. **Criar dashboard administrativo básico** (3-4 horas)

### **Prioridade 2 (Melhorias):**
1. **Validação de antecedência de reservas**
2. **Configuração de horários do restaurante**
3. **Código de confirmação para reservas**
4. **Painel administrativo mais completo**

### **Prioridade 3 (Opcional):**
1. **Email de confirmação para reservas/pedidos**
2. **Upload de imagens para itens do cardápio**
3. **Sistema de avaliações**
4. **Relatórios mais avançados**

---

## Como Testar o Sistema Atual

### **Backend:**
```bash
cd GestaoDeRestaurante
dotnet run
```
- API disponível em: `https://localhost:7xxx`
- Swagger: `https://localhost:7xxx/swagger`

### **Banco de Dados:**
- **Usuário admin**: `admin@restaurante.com` / `senha123`
- **Usuários comuns**: `carlos@email.com` / `senha123` (e outros)

### **Frontend (React):**
Seguir o guia em `GUIA_FRONTEND_REACT.md`

---

## Conclusão

O projeto está **90% completo** do lado do backend. A parte principal que falta é o **sistema de administração** com diferenciação de roles/permissões. 

**Pontos fortes:**
- Arquitetura bem organizada (Models, Controllers, Services, DTOs)
- Todas as regras de negócio principais implementadas
- Banco de dados com seed completo
- API documentada com Swagger
- Autenticação JWT funcionando

**Pontos a melhorar:**
- Sistema de autorização (roles)
- Painel administrativo
- Algumas validações específicas de negócio

Com as implementações sugeridas acima (4-8 horas de trabalho), o sistema estará 100% completo e pronto para entrega.