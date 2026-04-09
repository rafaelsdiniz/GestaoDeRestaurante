-- ============================================================
-- SEED DATA - GestaoDeRestaurante
-- Execute após rodar: dotnet ef database update
-- ============================================================

USE GestaoRestaurante;
GO

-- ============================================================
-- USUARIOS (Senhas: todas são "senha123" hasheadas com BCrypt)
-- TipoUsuario: 1 = Cliente, 2 = Administrador
-- ============================================================
INSERT INTO Usuarios (Nome, Email, Senha, TipoUsuario) VALUES
('Admin Restaurante',   'admin@restaurante.com',   '$2a$11$pTKNuULkO0RD87P..KLNM.rp4cxdPxXV3xGuLqcv1cXpL7SMfLKs.', 2),
('Carlos Silva',        'carlos@email.com',         '$2a$11$pTKNuULkO0RD87P..KLNM.rp4cxdPxXV3xGuLqcv1cXpL7SMfLKs.', 1),
('Ana Oliveira',        'ana@email.com',             '$2a$11$pTKNuULkO0RD87P..KLNM.rp4cxdPxXV3xGuLqcv1cXpL7SMfLKs.', 1),
('Pedro Souza',         'pedro@email.com',           '$2a$11$pTKNuULkO0RD87P..KLNM.rp4cxdPxXV3xGuLqcv1cXpL7SMfLKs.', 1),
('Mariana Costa',       'mariana@email.com',         '$2a$11$pTKNuULkO0RD87P..KLNM.rp4cxdPxXV3xGuLqcv1cXpL7SMfLKs.', 1);
GO

-- ============================================================
-- CORRIGE SENHAS (roda sempre, mesmo se Usuarios já existia)
-- Todas as senhas = "senha123"
-- ============================================================
UPDATE Usuarios
SET Senha = '$2a$11$pTKNuULkO0RD87P..KLNM.rp4cxdPxXV3xGuLqcv1cXpL7SMfLKs.'
WHERE Senha <> '$2a$11$pTKNuULkO0RD87P..KLNM.rp4cxdPxXV3xGuLqcv1cXpL7SMfLKs.'
   OR Senha IS NULL;
GO

-- ============================================================
-- GARANTE QUE ADMIN É ADMINISTRADOR
-- ============================================================
UPDATE Usuarios
SET TipoUsuario = 2
WHERE Email = 'admin@restaurante.com';
GO

-- ============================================================
-- ENDERECOS
-- ============================================================
INSERT INTO Enderecos (Rua, Numero, Complemento, Bairro, Cidade, Estado, Cep, UsuarioId) VALUES
('Rua das Flores',      '123',  'Apto 4B',  'Centro',       'São Paulo',    'SP', '01310-100', 2),
('Av. Paulista',        '1500', NULL,        'Bela Vista',   'São Paulo',    'SP', '01310-200', 3),
('Rua XV de Novembro',  '200',  'Casa',      'Centro',       'Curitiba',     'PR', '80020-310', 4),
('Rua do Comércio',     '45',   NULL,        'Pinheiros',    'São Paulo',    'SP', '05422-001', 5);
GO

-- ============================================================
-- MESAS
-- ============================================================
INSERT INTO Mesas (Numero, Capacidade) VALUES
(1,  2),
(2,  2),
(3,  4),
(4,  4),
(5,  4),
(6,  6),
(7,  6),
(8,  8),
(9,  8),
(10, 10);
GO

-- ============================================================
-- INGREDIENTES
-- ============================================================
INSERT INTO Ingredientes (Nome) VALUES
('Frango'),
('Carne Bovina'),
('Peixe'),
('Camarão'),
('Arroz'),
('Feijão'),
('Macarrão'),
('Tomate'),
('Cebola'),
('Alho'),
('Pimentão'),
('Queijo'),
('Presunto'),
('Bacon'),
('Alface'),
('Azeite de Oliva'),
('Limão'),
('Cream Cheese'),
('Pão Francês'),
('Batata');
GO

-- ============================================================
-- ITENS DO CARDAPIO
-- Periodo: 1 = Almoco, 2 = Jantar
-- ============================================================
INSERT INTO ItensCardapio (Nome, Descricao, PrecoBase, Periodo) VALUES
-- Almoço (20 itens)
('Frango Grelhado com Arroz',       'Peito de frango grelhado temperado com ervas, acompanha arroz e feijão.',              32.90, 1),
('Picanha na Chapa',                'Picanha grelhada no ponto, acompanha arroz, feijão e farofa.',                         58.90, 1),
('Peixe Grelhado',                  'Filé de tilápia grelhado com limão e ervas, acompanha arroz e legumes.',               44.90, 1),
('Macarronada ao Molho Bolonhesa',  'Macarrão espaguete ao molho de carne moída, servido com queijo parmesão.',             28.90, 1),
('Salada Caesar com Frango',        'Alface romana, croutons, queijo parmesão, molho caesar e frango grelhado.',            27.50, 1),
('Misto Quente Especial',           'Pão quentinho com queijo e presunto, acompanha batata frita.',                         22.90, 1),
('Feijoada Completa',               'Feijoada com carnes selecionadas, acompanha arroz, couve, farofa e laranja.',          42.90, 1),
('Strogonoff de Frango',            'Strogonoff cremoso de frango com arroz branco e batata palha.',                        34.90, 1),
('Escondidinho de Carne Seca',      'Purê de mandioca gratinado recheado com carne seca desfiada.',                         36.90, 1),
('Bife Acebolado',                  'Bife de alcatra grelhado com cebolas caramelizadas, arroz e feijão.',                  38.90, 1),
('Lasanha de Frango',               'Lasanha com camadas de frango desfiado, molho branco e queijo gratinado.',             32.50, 1),
('Galinhada Caipira',               'Arroz temperado com frango caipira, pequi e açafrão.',                                35.90, 1),
('Parmegiana de Frango',            'Filé de frango empanado com molho de tomate e queijo gratinado, arroz e fritas.',      37.90, 1),
('Arroz Carreteiro',                'Arroz com charque desfiado, temperos e legumes.',                                      29.90, 1),
('Filé de Frango à Milanesa',       'Filé empanado crocante acompanha arroz, feijão e salada.',                             31.90, 1),
('Hambúrguer Artesanal',            'Hambúrguer artesanal com queijo, alface, tomate e batata frita.',                      33.90, 1),
('Wrap de Frango',                  'Wrap de tortilha com frango grelhado, alface, tomate e molho especial.',               26.90, 1),
('Nhoque ao Sugo',                  'Nhoque de batata ao molho de tomate fresco com manjericão.',                           30.90, 1),
('Quiche de Queijo e Presunto',     'Quiche assada com recheio de queijo e presunto, acompanha salada.',                    25.90, 1),
('Torta de Frango',                 'Torta caseira recheada com frango desfiado e catupiry.',                               24.90, 1),
-- Jantar (20 itens)
('Salmão ao Molho de Alcaparras',   'Salmão fresco ao forno com molho de alcaparras e azeite, acompanha legumes.',         68.90, 2),
('Camarão na Moranga',              'Camarões salteados no alho e azeite servidos dentro de moranga recheada.',             72.90, 2),
('Filé Mignon ao Molho Madeira',    'Medalhão de filé mignon ao molho madeira com batatas rústicas.',                      79.90, 2),
('Risoto de Camarão',               'Risoto cremoso com camarões frescos, tomate e cream cheese.',                         64.90, 2),
('Frango Recheado com Cream Cheese','Peito de frango recheado com cream cheese e bacon, acompanha arroz e legumes.',       48.90, 2),
('Costela ao Forno',                'Costela de boi assada lentamente com temperos especiais, acompanha arroz e mandioca.',67.90, 2),
('Cordeiro Assado',                 'Pernil de cordeiro assado com ervas finas e batatas ao murro.',                        89.90, 2),
('Lagosta Grelhada',                'Lagosta grelhada na manteiga com ervas, acompanha risoto de limão.',                   99.90, 2),
('Polvo à Lagareiro',               'Polvo grelhado com azeite, alho e batatas a murro.',                                  92.90, 2),
('Risoto de Funghi',                'Risoto cremoso com mix de cogumelos e queijo parmesão.',                               58.90, 2),
('Magret de Pato',                  'Peito de pato grelhado com molho de laranja e purê de batata.',                        85.90, 2),
('Ravioli de Trufa',                'Ravioli artesanal recheado com trufa negra ao molho de manteiga e sálvia.',            76.90, 2),
('Atum Selado',                     'Atum fresco selado com crosta de gergelim, acompanha legumes salteados.',              74.90, 2),
('Ossobuco',                        'Ossobuco de vitela cozido lentamente em molho de vinho, com risoto de açafrão.',       82.90, 2),
('Vieiras Gratinadas',              'Vieiras frescas gratinadas com queijo gruyère e ervas finas.',                         88.90, 2),
('Pappardelle ao Ragu',             'Massa fresca pappardelle com ragu de carne bovina cozido por 8 horas.',               62.90, 2),
('Bacalhau à Brás',                 'Bacalhau desfiado com batata palha, ovos e azeitonas.',                                72.90, 2),
('Leitão à Pururuca',               'Leitão assado com pele crocante, acompanha arroz, feijão tropeiro e couve.',           78.90, 2),
('Tournedos Rossini',               'Medalhão de filé mignon com foie gras e molho de trufa.',                              95.90, 2),
('Paella Valenciana',               'Paella com camarão, lula, mexilhão, frango e legumes ao açafrão.',                     86.90, 2);
GO

-- ============================================================
-- ITEM INGREDIENTE (relação N:N)
-- ItemCardapioId x IngredienteId
-- ============================================================
-- Frango Grelhado (id=1): Frango(1), Arroz(5), Feijão(6), Alho(10), Azeite(16)
INSERT INTO ItemIngredientes (ItemCardapioId, IngredienteId) VALUES
(1, 1), (1, 5), (1, 6), (1, 10), (1, 16),
-- Picanha (id=2): Carne(2), Arroz(5), Feijão(6), Alho(10)
(2, 2), (2, 5), (2, 6), (2, 10),
-- Peixe (id=3): Peixe(3), Arroz(5), Limão(17), Azeite(16)
(3, 3), (3, 5), (3, 17), (3, 16),
-- Macarronada (id=4): Carne(2), Macarrão(7), Tomate(8), Cebola(9), Queijo(12)
(4, 2), (4, 7), (4, 8), (4, 9), (4, 12),
-- Salada Caesar (id=5): Frango(1), Alface(15), Queijo(12), Limão(17)
(5, 1), (5, 15), (5, 12), (5, 17),
-- Misto Quente (id=6): Queijo(12), Presunto(13), Pão(19), Batata(20)
(6, 12), (6, 13), (6, 19), (6, 20),
-- Salmão (id=7): Peixe(3), Azeite(16), Limão(17), Alho(10)
(7, 3), (7, 16), (7, 17), (7, 10),
-- Camarão na Moranga (id=8): Camarão(4), Alho(10), Azeite(16), Cebola(9)
(8, 4), (8, 10), (8, 16), (8, 9),
-- Filé Mignon (id=9): Carne(2), Batata(20), Alho(10), Azeite(16)
(9, 2), (9, 20), (9, 10), (9, 16),
-- Risoto de Camarão (id=10): Camarão(4), Arroz(5), Tomate(8), Cream Cheese(18)
(10, 4), (10, 5), (10, 8), (10, 18),
-- Frango Recheado (id=11): Frango(1), Cream Cheese(18), Bacon(14), Arroz(5)
(11, 1), (11, 18), (11, 14), (11, 5),
-- Costela (id=12): Carne(2), Alho(10), Cebola(9), Batata(20)
(12, 2), (12, 10), (12, 9), (12, 20),
-- Feijoada (id=13): Carne(2), Feijão(6), Arroz(5), Alho(10)
(13, 2), (13, 6), (13, 5), (13, 10),
-- Strogonoff (id=14): Frango(1), Arroz(5), Tomate(8), Cebola(9)
(14, 1), (14, 5), (14, 8), (14, 9),
-- Escondidinho (id=15): Carne(2), Batata(20), Queijo(12), Cebola(9)
(15, 2), (15, 20), (15, 12), (15, 9),
-- Bife Acebolado (id=16): Carne(2), Cebola(9), Arroz(5), Feijão(6)
(16, 2), (16, 9), (16, 5), (16, 6),
-- Lasanha de Frango (id=17): Frango(1), Queijo(12), Tomate(8), Macarrão(7)
(17, 1), (17, 12), (17, 8), (17, 7),
-- Galinhada (id=18): Frango(1), Arroz(5), Alho(10), Cebola(9)
(18, 1), (18, 5), (18, 10), (18, 9),
-- Parmegiana (id=19): Frango(1), Queijo(12), Tomate(8), Arroz(5)
(19, 1), (19, 12), (19, 8), (19, 5),
-- Arroz Carreteiro (id=20): Carne(2), Arroz(5), Cebola(9), Pimentão(11)
(20, 2), (20, 5), (20, 9), (20, 11),
-- Filé à Milanesa (id=21): Frango(1), Arroz(5), Feijão(6), Alface(15)
(21, 1), (21, 5), (21, 6), (21, 15),
-- Hambúrguer (id=22): Carne(2), Queijo(12), Alface(15), Tomate(8), Batata(20)
(22, 2), (22, 12), (22, 15), (22, 8), (22, 20),
-- Wrap (id=23): Frango(1), Alface(15), Tomate(8), Queijo(12)
(23, 1), (23, 15), (23, 8), (23, 12),
-- Nhoque (id=24): Batata(20), Tomate(8), Alho(10), Azeite(16)
(24, 20), (24, 8), (24, 10), (24, 16),
-- Quiche (id=25): Queijo(12), Presunto(13), Cebola(9)
(25, 12), (25, 13), (25, 9),
-- Torta de Frango (id=26): Frango(1), Queijo(12), Tomate(8), Cebola(9)
(26, 1), (26, 12), (26, 8), (26, 9),
-- Cordeiro (id=27): Carne(2), Batata(20), Alho(10), Azeite(16)
(27, 2), (27, 20), (27, 10), (27, 16),
-- Lagosta (id=28): Camarão(4), Arroz(5), Limão(17), Azeite(16)
(28, 4), (28, 5), (28, 17), (28, 16),
-- Polvo (id=29): Peixe(3), Batata(20), Alho(10), Azeite(16)
(29, 3), (29, 20), (29, 10), (29, 16),
-- Risoto Funghi (id=30): Arroz(5), Queijo(12), Azeite(16), Cebola(9)
(30, 5), (30, 12), (30, 16), (30, 9),
-- Magret de Pato (id=31): Frango(1), Batata(20), Limão(17), Azeite(16)
(31, 1), (31, 20), (31, 17), (31, 16),
-- Ravioli (id=32): Macarrão(7), Queijo(12), Azeite(16)
(32, 7), (32, 12), (32, 16),
-- Atum (id=33): Peixe(3), Azeite(16), Limão(17), Alho(10)
(33, 3), (33, 16), (33, 17), (33, 10),
-- Ossobuco (id=34): Carne(2), Arroz(5), Tomate(8), Cebola(9)
(34, 2), (34, 5), (34, 8), (34, 9),
-- Vieiras (id=35): Camarão(4), Queijo(12), Alho(10), Azeite(16)
(35, 4), (35, 12), (35, 10), (35, 16),
-- Pappardelle (id=36): Macarrão(7), Carne(2), Tomate(8), Cebola(9)
(36, 7), (36, 2), (36, 8), (36, 9),
-- Bacalhau (id=37): Peixe(3), Batata(20), Cebola(9), Azeite(16)
(37, 3), (37, 20), (37, 9), (37, 16),
-- Leitão (id=38): Carne(2), Arroz(5), Feijão(6), Alho(10)
(38, 2), (38, 5), (38, 6), (38, 10),
-- Tournedos (id=39): Carne(2), Queijo(12), Azeite(16), Alho(10)
(39, 2), (39, 12), (39, 16), (39, 10),
-- Paella (id=40): Camarão(4), Frango(1), Arroz(5), Pimentão(11), Alho(10)
(40, 4), (40, 1), (40, 5), (40, 11), (40, 10);
GO

-- ============================================================
-- SUGESTOES DO CHEFE
-- Periodo: 1=Almoco, 2=Jantar
-- ============================================================
INSERT INTO SugestoesChefe (DataSugestao, Periodo, ItemCardapioId) VALUES
('2026-03-25', 1, 2),   -- Picanha no Almoço
('2026-03-25', 2, 8),   -- Camarão na Moranga no Jantar
('2026-03-26', 1, 1),   -- Frango Grelhado no Almoço
('2026-03-26', 2, 7),   -- Salmão no Jantar
('2026-03-27', 1, 4),   -- Macarronada no Almoço
('2026-03-27', 2, 9),   -- Filé Mignon no Jantar
('2026-03-28', 1, 3),   -- Peixe no Almoço
('2026-03-28', 2, 10),  -- Risoto no Jantar
('2026-03-29', 1, 5),   -- Salada Caesar no Almoço
('2026-03-29', 2, 12),  -- Costela no Jantar
('2026-03-30', 1, 2),   -- Picanha no Almoço (hoje)
('2026-03-30', 2, 11);  -- Frango Recheado no Jantar (hoje)
GO

-- ============================================================
-- RESERVAS
-- StatusReserva: 1=Ativa, 2=Cancelada, 3=Finalizada, 4=Confirmada
-- ============================================================
INSERT INTO Reservas (DataHoraReserva, QuantidadePessoas, StatusReserva, CodigoConfirmacao, UsuarioID, MesaId) VALUES
('2026-03-30 11:30:00', 2, 4, 'CONF0001', 2, 1),
('2026-03-30 12:00:00', 4, 1, 'RESV0002', 3, 3),
('2026-03-30 12:30:00', 2, 4, 'CONF0003', 4, 2),
('2026-03-30 13:00:00', 6, 1, 'RESV0004', 5, 6),
('2026-03-29 12:00:00', 4, 3, 'CONF0005', 2, 4),
('2026-03-29 13:00:00', 2, 3, 'CONF0006', 3, 1),
('2026-03-28 11:30:00', 8, 3, 'CONF0007', 4, 8),
('2026-03-31 12:00:00', 4, 1, 'RESV0008', 5, 5),
('2026-03-31 13:00:00', 6, 1, 'RESV0009', 2, 7),
('2026-04-01 12:00:00', 2, 1, 'RESV0010', 3, 2);
GO

-- ============================================================
-- ATENDIMENTOS
-- TipoAtendimento: 1=Presencial, 2=DeliveryProprio, 3=DeliveryAplicativo
-- Discriminator column (TPH inheritance)
-- ============================================================
INSERT INTO Atendimentos (TipoAtendimento, DataHora, TaxaEntrega, Discriminator, ObservacaoEntrega, NomeAplicativo) VALUES
(1, '2026-03-25 12:30:00', 0.00,  'AtendimentoPresencial',       NULL,                   NULL),
(1, '2026-03-25 13:00:00', 0.00,  'AtendimentoPresencial',       NULL,                   NULL),
(1, '2026-03-25 20:00:00', 0.00,  'AtendimentoPresencial',       NULL,                   NULL),
(2, '2026-03-26 12:15:00', 10.00, 'AtendimentoDeliveryProprio',  'Entregar no portão',   NULL),
(2, '2026-03-26 19:45:00', 10.00, 'AtendimentoDeliveryProprio',  'Ligar ao chegar',      NULL),
(3, '2026-03-27 12:00:00', 0.00,  'AtendimentoDeliveryAplicativo', NULL,                 'iFood'),
(3, '2026-03-27 20:30:00', 0.00,  'AtendimentoDeliveryAplicativo', NULL,                 'Rappi'),
(1, '2026-03-28 12:00:00', 0.00,  'AtendimentoPresencial',       NULL,                   NULL),
(3, '2026-03-29 19:00:00', 0.00,  'AtendimentoDeliveryAplicativo', NULL,                 'iFood'),
(2, '2026-03-30 12:30:00', 10.00, 'AtendimentoDeliveryProprio',  NULL,                   NULL);
GO

-- ============================================================
-- PEDIDOS
-- Periodo: 1=Almoco, 2=Jantar
-- ============================================================
INSERT INTO Pedidos (DataHora, Periodo, Subtotal, Desconto, TaxaEntrega, Total, UsuarioId, AtendimentoId) VALUES
('2026-03-25 12:30:00', 1, 91.80,  0.00,  0.00,  91.80,  2, 1),
('2026-03-25 13:00:00', 1, 58.90,  0.00,  0.00,  58.90,  3, 2),
('2026-03-25 20:00:00', 2, 147.80, 0.00,  0.00,  147.80, 4, 3),
('2026-03-26 12:15:00', 1, 61.80,  0.00,  10.00, 71.80,  5, 4),
('2026-03-26 19:45:00', 2, 72.90,  0.00,  10.00, 82.90,  2, 5),
('2026-03-27 12:00:00', 1, 32.90,  0.00,  0.00,  32.90,  3, 6),
('2026-03-27 20:30:00', 2, 64.90,  0.00,  0.00,  64.90,  4, 7),
('2026-03-28 12:00:00', 1, 103.80, 5.00,  0.00,  98.80,  5, 8),
('2026-03-29 19:00:00', 2, 68.90,  0.00,  0.00,  68.90,  2, 9),
('2026-03-30 12:30:00', 1, 87.80,  0.00,  10.00, 97.80,  3, 10);
GO

-- ============================================================
-- ITENS PEDIDO
-- PrecoUnitario = PrecoBase do item; Subtotal = qty * preco
-- ============================================================
INSERT INTO ItensPedidos (PedidoId, ItemCardapioId, Quantidade, PrecoUnitario, Subtotal) VALUES
-- Pedido 1 (Carlos, Presencial, Almoço): Frango + Picanha
(1, 1, 1, 32.90, 32.90),
(1, 2, 1, 58.90, 58.90),
-- Pedido 2 (Ana, Presencial, Almoço): Picanha
(2, 2, 1, 58.90, 58.90),
-- Pedido 3 (Pedro, Presencial, Jantar): Salmão + Filé Mignon
(3, 7, 1, 68.90, 68.90),
(3, 9, 1, 79.90, 79.90),
-- Pedido 4 (Mariana, DeliveryProprio, Almoço): Frango + Macarronada
(4, 1, 1, 32.90, 32.90),
(4, 4, 1, 28.90, 28.90),
-- Pedido 5 (Carlos, DeliveryProprio, Jantar): Camarão na Moranga
(5, 8, 1, 72.90, 72.90),
-- Pedido 6 (Ana, iFood, Almoço): Frango
(6, 1, 1, 32.90, 32.90),
-- Pedido 7 (Pedro, Rappi, Jantar): Risoto
(7, 10, 1, 64.90, 64.90),
-- Pedido 8 (Mariana, Presencial, Almoço): 2x Frango + Picanha (desconto 5)
(8, 1, 2, 32.90, 65.80),
(8, 2, 1, 58.90, 58.90),  -- nota: subtotal pedido considera desconto no total, não no item
-- Pedido 9 (Carlos, iFood, Jantar): Salmão
(9, 7, 1, 68.90, 68.90),
-- Pedido 10 (Ana, DeliveryProprio, Almoço): Picanha + Salada
(10, 2, 1, 58.90, 58.90),
(10, 5, 1, 27.50, 27.50);
GO

-- ============================================================
-- VERIFICACAO RAPIDA
-- ============================================================
SELECT 'Usuarios'        AS Tabela, COUNT(*) AS Total FROM Usuarios
UNION ALL
SELECT 'Enderecos',       COUNT(*) FROM Enderecos
UNION ALL
SELECT 'Mesas',           COUNT(*) FROM Mesas
UNION ALL
SELECT 'Ingredientes',    COUNT(*) FROM Ingredientes
UNION ALL
SELECT 'ItensCardapio',   COUNT(*) FROM ItensCardapio
UNION ALL
SELECT 'ItemIngrediente', COUNT(*) FROM ItemIngredientes
UNION ALL
SELECT 'SugestaoChefe',   COUNT(*) FROM SugestoesChefe
UNION ALL
SELECT 'Reservas',        COUNT(*) FROM Reservas
UNION ALL
SELECT 'Atendimentos',    COUNT(*) FROM Atendimentos
UNION ALL
SELECT 'Pedidos',         COUNT(*) FROM Pedidos
UNION ALL
SELECT 'ItensPedido',     COUNT(*) FROM ItensPedidos;
GO
