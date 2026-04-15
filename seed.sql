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
SET IDENTITY_INSERT Usuarios ON;
INSERT INTO Usuarios (Id, Nome, Email, Senha, TipoUsuario) VALUES
(1, 'Admin Restaurante',   'admin@restaurante.com',   '$2a$11$pTKNuULkO0RD87P..KLNM.rp4cxdPxXV3xGuLqcv1cXpL7SMfLKs.', 2),
(2, 'Carlos Silva',        'carlos@email.com',         '$2a$11$pTKNuULkO0RD87P..KLNM.rp4cxdPxXV3xGuLqcv1cXpL7SMfLKs.', 1),
(3, 'Ana Oliveira',        'ana@email.com',             '$2a$11$pTKNuULkO0RD87P..KLNM.rp4cxdPxXV3xGuLqcv1cXpL7SMfLKs.', 1),
(4, 'Pedro Souza',         'pedro@email.com',           '$2a$11$pTKNuULkO0RD87P..KLNM.rp4cxdPxXV3xGuLqcv1cXpL7SMfLKs.', 1),
(5, 'Mariana Costa',       'mariana@email.com',         '$2a$11$pTKNuULkO0RD87P..KLNM.rp4cxdPxXV3xGuLqcv1cXpL7SMfLKs.', 1);
SET IDENTITY_INSERT Usuarios OFF;
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
SET IDENTITY_INSERT Enderecos ON;
INSERT INTO Enderecos (Id, Rua, Numero, Complemento, Bairro, Cidade, Estado, Cep, UsuarioId) VALUES
(1, 'Rua das Flores',      '123',  'Apto 4B',  'Centro',       'São Paulo',    'SP', '01310-100', 2),
(2, 'Av. Paulista',        '1500', NULL,        'Bela Vista',   'São Paulo',    'SP', '01310-200', 3),
(3, 'Rua XV de Novembro',  '200',  'Casa',      'Centro',       'Curitiba',     'PR', '80020-310', 4),
(4, 'Rua do Comércio',     '45',   NULL,        'Pinheiros',    'São Paulo',    'SP', '05422-001', 5);
SET IDENTITY_INSERT Enderecos OFF;
GO

-- ============================================================
-- MESAS
-- ============================================================
SET IDENTITY_INSERT Mesas ON;
INSERT INTO Mesas (Id, Numero, Capacidade) VALUES
(1,  1,  2),
(2,  2,  2),
(3,  3,  4),
(4,  4,  4),
(5,  5,  4),
(6,  6,  6),
(7,  7,  6),
(8,  8,  8),
(9,  9,  8),
(10, 10, 10);
SET IDENTITY_INSERT Mesas OFF;
GO

-- ============================================================
-- INGREDIENTES
-- ============================================================
SET IDENTITY_INSERT Ingredientes ON;
INSERT INTO Ingredientes (Id, Nome) VALUES
(1,  'Frango'),
(2,  'Carne Bovina'),
(3,  'Peixe'),
(4,  'Camarão'),
(5,  'Arroz'),
(6,  'Feijão'),
(7,  'Macarrão'),
(8,  'Tomate'),
(9,  'Cebola'),
(10, 'Alho'),
(11, 'Pimentão'),
(12, 'Queijo'),
(13, 'Presunto'),
(14, 'Bacon'),
(15, 'Alface'),
(16, 'Azeite de Oliva'),
(17, 'Limão'),
(18, 'Cream Cheese'),
(19, 'Pão Francês'),
(20, 'Batata'),
(21, 'Laranja'),
(22, 'Maracujá'),
(23, 'Morango'),
(24, 'Abacaxi'),
(25, 'Manga'),
(26, 'Hortelã'),
(27, 'Gengibre'),
(28, 'Leite'),
(29, 'Café'),
(30, 'Açúcar'),
(31, 'Cacau'),
(32, 'Mel'),
(33, 'Canela'),
(34, 'Água com Gás'),
(35, 'Leite Condensado');
SET IDENTITY_INSERT Ingredientes OFF;
GO

-- ============================================================
-- ITENS DO CARDAPIO
-- Periodo: 1 = Almoco, 2 = Jantar
-- ============================================================
SET IDENTITY_INSERT ItensCardapio ON;
INSERT INTO ItensCardapio (Id, Nome, Descricao, PrecoBase, Periodo, Categoria) VALUES
-- Almoço - Pratos (id 1-20) | Periodo=1(Almoco), Categoria=1(Prato)
(1,  'Frango Grelhado com Arroz',       'Peito de frango grelhado temperado com ervas, acompanha arroz e feijão.',              32.90, 1, 1),
(2,  'Picanha na Chapa',                'Picanha grelhada no ponto, acompanha arroz, feijão e farofa.',                         58.90, 1, 1),
(3,  'Peixe Grelhado',                  'Filé de tilápia grelhado com limão e ervas, acompanha arroz e legumes.',               44.90, 1, 1),
(4,  'Macarronada ao Molho Bolonhesa',  'Macarrão espaguete ao molho de carne moída, servido com queijo parmesão.',             28.90, 1, 1),
(5,  'Salada Caesar com Frango',        'Alface romana, croutons, queijo parmesão, molho caesar e frango grelhado.',            27.50, 1, 1),
(6,  'Misto Quente Especial',           'Pão quentinho com queijo e presunto, acompanha batata frita.',                         22.90, 1, 1),
(7,  'Feijoada Completa',               'Feijoada com carnes selecionadas, acompanha arroz, couve, farofa e laranja.',          42.90, 1, 1),
(8,  'Strogonoff de Frango',            'Strogonoff cremoso de frango com arroz branco e batata palha.',                        34.90, 1, 1),
(9,  'Escondidinho de Carne Seca',      'Purê de mandioca gratinado recheado com carne seca desfiada.',                         36.90, 1, 1),
(10, 'Bife Acebolado',                  'Bife de alcatra grelhado com cebolas caramelizadas, arroz e feijão.',                  38.90, 1, 1),
(11, 'Lasanha de Frango',               'Lasanha com camadas de frango desfiado, molho branco e queijo gratinado.',             32.50, 1, 1),
(12, 'Galinhada Caipira',               'Arroz temperado com frango caipira, pequi e açafrão.',                                35.90, 1, 1),
(13, 'Parmegiana de Frango',            'Filé de frango empanado com molho de tomate e queijo gratinado, arroz e fritas.',      37.90, 1, 1),
(14, 'Arroz Carreteiro',                'Arroz com charque desfiado, temperos e legumes.',                                      29.90, 1, 1),
(15, 'Filé de Frango à Milanesa',       'Filé empanado crocante acompanha arroz, feijão e salada.',                             31.90, 1, 1),
(16, 'Hambúrguer Artesanal',            'Hambúrguer artesanal com queijo, alface, tomate e batata frita.',                      33.90, 1, 1),
(17, 'Wrap de Frango',                  'Wrap de tortilha com frango grelhado, alface, tomate e molho especial.',               26.90, 1, 1),
(18, 'Nhoque ao Sugo',                  'Nhoque de batata ao molho de tomate fresco com manjericão.',                           30.90, 1, 1),
(19, 'Quiche de Queijo e Presunto',     'Quiche assada com recheio de queijo e presunto, acompanha salada.',                    25.90, 1, 1),
(20, 'Torta de Frango',                 'Torta caseira recheada com frango desfiado e catupiry.',                               24.90, 1, 1),
-- Jantar - Pratos (id 21-40) | Periodo=2(Jantar), Categoria=1(Prato)
(21, 'Salmão ao Molho de Alcaparras',   'Salmão fresco ao forno com molho de alcaparras e azeite, acompanha legumes.',         68.90, 2, 1),
(22, 'Camarão na Moranga',              'Camarões salteados no alho e azeite servidos dentro de moranga recheada.',             72.90, 2, 1),
(23, 'Filé Mignon ao Molho Madeira',    'Medalhão de filé mignon ao molho madeira com batatas rústicas.',                      79.90, 2, 1),
(24, 'Risoto de Camarão',               'Risoto cremoso com camarões frescos, tomate e cream cheese.',                         64.90, 2, 1),
(25, 'Frango Recheado com Cream Cheese','Peito de frango recheado com cream cheese e bacon, acompanha arroz e legumes.',       48.90, 2, 1),
(26, 'Costela ao Forno',                'Costela de boi assada lentamente com temperos especiais, acompanha arroz e mandioca.',67.90, 2, 1),
(27, 'Cordeiro Assado',                 'Pernil de cordeiro assado com ervas finas e batatas ao murro.',                        89.90, 2, 1),
(28, 'Lagosta Grelhada',                'Lagosta grelhada na manteiga com ervas, acompanha risoto de limão.',                   99.90, 2, 1),
(29, 'Polvo à Lagareiro',               'Polvo grelhado com azeite, alho e batatas a murro.',                                  92.90, 2, 1),
(30, 'Risoto de Funghi',                'Risoto cremoso com mix de cogumelos e queijo parmesão.',                               58.90, 2, 1),
(31, 'Magret de Pato',                  'Peito de pato grelhado com molho de laranja e purê de batata.',                        85.90, 2, 1),
(32, 'Ravioli de Trufa',                'Ravioli artesanal recheado com trufa negra ao molho de manteiga e sálvia.',            76.90, 2, 1),
(33, 'Atum Selado',                     'Atum fresco selado com crosta de gergelim, acompanha legumes salteados.',              74.90, 2, 1),
(34, 'Ossobuco',                        'Ossobuco de vitela cozido lentamente em molho de vinho, com risoto de açafrão.',       82.90, 2, 1),
(35, 'Vieiras Gratinadas',              'Vieiras frescas gratinadas com queijo gruyère e ervas finas.',                         88.90, 2, 1),
(36, 'Pappardelle ao Ragu',             'Massa fresca pappardelle com ragu de carne bovina cozido por 8 horas.',               62.90, 2, 1),
(37, 'Bacalhau à Brás',                 'Bacalhau desfiado com batata palha, ovos e azeitonas.',                                72.90, 2, 1),
(38, 'Leitão à Pururuca',               'Leitão assado com pele crocante, acompanha arroz, feijão tropeiro e couve.',           78.90, 2, 1),
(39, 'Tournedos Rossini',               'Medalhão de filé mignon com foie gras e molho de trufa.',                              95.90, 2, 1),
(40, 'Paella Valenciana',               'Paella com camarão, lula, mexilhão, frango e legumes ao açafrão.',                     86.90, 2, 1),
-- Bebidas Almoço (id 41-55) | Periodo=1(Almoco), Categoria=2(Bebida)
(41, 'Suco de Laranja Natural',         'Suco de laranja espremido na hora, 400ml.',                                            9.90,  1, 2),
(42, 'Suco de Maracujá',                'Suco de maracujá natural com açúcar, 400ml.',                                          9.90,  1, 2),
(43, 'Suco de Morango',                 'Suco de morango fresco batido, 400ml.',                                                10.90, 1, 2),
(44, 'Suco de Abacaxi com Hortelã',     'Suco de abacaxi fresco com hortelã, refrescante, 400ml.',                              10.90, 1, 2),
(45, 'Suco de Manga',                   'Suco de manga natural cremoso, 400ml.',                                                9.90,  1, 2),
(46, 'Limonada Suíça',                  'Limonada batida com leite condensado e gelo, 400ml.',                                  11.90, 1, 2),
(47, 'Refrigerante Lata',               'Coca-Cola, Guaraná ou Sprite, lata 350ml.',                                            7.90,  1, 2),
(48, 'Água Mineral',                    'Água mineral sem gás, 500ml.',                                                         5.90,  1, 2),
(49, 'Água com Gás',                    'Água mineral com gás, 500ml.',                                                         6.90,  1, 2),
(50, 'Café Expresso',                   'Café expresso curto ou longo, preparado na hora.',                                      6.90,  1, 2),
(51, 'Cappuccino',                      'Cappuccino cremoso com leite vaporizado e canela.',                                    10.90, 1, 2),
(52, 'Chá Gelado de Pêssego',           'Chá gelado sabor pêssego, refrescante, 400ml.',                                        8.90,  1, 2),
(53, 'Vitamina de Morango',             'Vitamina de morango com leite e mel, 400ml.',                                          12.90, 1, 2),
(54, 'Chocolate Quente',                'Chocolate quente cremoso com leite e cacau, 300ml.',                                   11.90, 1, 2),
(55, 'Cerveja Pilsen 600ml',            'Cerveja pilsen gelada, garrafa 600ml.',                                                14.90, 1, 2),
-- Bebidas Jantar (id 56-70) | Periodo=2(Jantar), Categoria=2(Bebida)
(56, 'Vinho Tinto Reserva - Taça',      'Taça de vinho tinto reserva encorpado, 150ml.',                                        28.90, 2, 2),
(57, 'Vinho Branco Seco - Taça',        'Taça de vinho branco seco refrescante, 150ml.',                                        26.90, 2, 2),
(58, 'Espumante Brut - Taça',           'Taça de espumante brut, ideal para celebrações, 150ml.',                               32.90, 2, 2),
(59, 'Sangria da Casa',                 'Sangria artesanal com vinho tinto, frutas frescas e especiarias, 400ml.',              22.90, 2, 2),
(60, 'Gin Tônica',                      'Gin artesanal com água tônica, limão e especiarias.',                                  29.90, 2, 2),
(61, 'Caipirinha de Limão',             'Caipirinha clássica de limão com cachaça artesanal.',                                  19.90, 2, 2),
(62, 'Caipirinha de Maracujá',          'Caipirinha de maracujá com cachaça artesanal.',                                        21.90, 2, 2),
(63, 'Whisky 12 Anos - Dose',           'Dose de whisky 12 anos, servido com gelo.',                                            34.90, 2, 2),
(64, 'Mojito',                          'Coquetel cubano com rum, hortelã fresca, limão e água com gás.',                       24.90, 2, 2),
(65, 'Aperol Spritz',                   'Aperol com espumante e água com gás, refrescante e leve.',                             27.90, 2, 2),
(66, 'Cerveja Artesanal IPA',           'Cerveja artesanal estilo IPA, 500ml.',                                                 19.90, 2, 2),
(67, 'Suco de Laranja Natural',         'Suco de laranja espremido na hora, opção sem álcool, 400ml.',                          12.90, 2, 2),
(68, 'Água Mineral com Gás',            'Água mineral com gás importada, 500ml.',                                                8.90, 2, 2),
(69, 'Café Expresso Duplo',             'Café expresso duplo para finalizar a refeição.',                                        9.90, 2, 2),
(70, 'Digestivo Limoncello',            'Dose de limoncello artesanal gelado, ideal como digestivo.',                            22.90, 2, 2);
SET IDENTITY_INSERT ItensCardapio OFF;
GO

-- ============================================================
-- CORRIGE CATEGORIA (roda sempre, mesmo se ItensCardapio já existia)
-- Categoria: 1=Prato, 2=Bebida
-- ============================================================
UPDATE ItensCardapio SET Categoria = 1 WHERE Id BETWEEN 1 AND 20 AND Categoria <> 1;
UPDATE ItensCardapio SET Categoria = 1 WHERE Id BETWEEN 21 AND 40 AND Categoria <> 1;
UPDATE ItensCardapio SET Categoria = 2 WHERE Id BETWEEN 41 AND 55 AND Categoria <> 2;
UPDATE ItensCardapio SET Categoria = 2 WHERE Id BETWEEN 56 AND 70 AND Categoria <> 2;
GO

-- ============================================================
-- ITEM INGREDIENTE (relação N:N)
-- ItemCardapioId x IngredienteId
-- ============================================================
-- === ALMOÇO (id 1-20) ===
-- Frango Grelhado (id=1): Frango(1), Arroz(5), Feijão(6), Alho(10), Azeite(16)
INSERT INTO ItemIngredientes (ItemCardapioId, IngredienteId) VALUES
(1, 1), (1, 5), (1, 6), (1, 10), (1, 16),
-- Picanha (id=2): Carne(2), Arroz(5), Feijão(6), Alho(10)
(2, 2), (2, 5), (2, 6), (2, 10),
-- Peixe Grelhado (id=3): Peixe(3), Arroz(5), Limão(17), Azeite(16)
(3, 3), (3, 5), (3, 17), (3, 16),
-- Macarronada (id=4): Carne(2), Macarrão(7), Tomate(8), Cebola(9), Queijo(12)
(4, 2), (4, 7), (4, 8), (4, 9), (4, 12),
-- Salada Caesar (id=5): Frango(1), Alface(15), Queijo(12), Limão(17)
(5, 1), (5, 15), (5, 12), (5, 17),
-- Misto Quente (id=6): Queijo(12), Presunto(13), Pão(19), Batata(20)
(6, 12), (6, 13), (6, 19), (6, 20),
-- Feijoada (id=7): Carne(2), Feijão(6), Arroz(5), Alho(10)
(7, 2), (7, 6), (7, 5), (7, 10),
-- Strogonoff (id=8): Frango(1), Arroz(5), Tomate(8), Cebola(9)
(8, 1), (8, 5), (8, 8), (8, 9),
-- Escondidinho (id=9): Carne(2), Batata(20), Queijo(12), Cebola(9)
(9, 2), (9, 20), (9, 12), (9, 9),
-- Bife Acebolado (id=10): Carne(2), Cebola(9), Arroz(5), Feijão(6)
(10, 2), (10, 9), (10, 5), (10, 6),
-- Lasanha de Frango (id=11): Frango(1), Queijo(12), Tomate(8), Macarrão(7)
(11, 1), (11, 12), (11, 8), (11, 7),
-- Galinhada (id=12): Frango(1), Arroz(5), Alho(10), Cebola(9)
(12, 1), (12, 5), (12, 10), (12, 9),
-- Parmegiana (id=13): Frango(1), Queijo(12), Tomate(8), Arroz(5)
(13, 1), (13, 12), (13, 8), (13, 5),
-- Arroz Carreteiro (id=14): Carne(2), Arroz(5), Cebola(9), Pimentão(11)
(14, 2), (14, 5), (14, 9), (14, 11),
-- Filé à Milanesa (id=15): Frango(1), Arroz(5), Feijão(6), Alface(15)
(15, 1), (15, 5), (15, 6), (15, 15),
-- Hambúrguer (id=16): Carne(2), Queijo(12), Alface(15), Tomate(8), Batata(20)
(16, 2), (16, 12), (16, 15), (16, 8), (16, 20),
-- Wrap (id=17): Frango(1), Alface(15), Tomate(8), Queijo(12)
(17, 1), (17, 15), (17, 8), (17, 12),
-- Nhoque (id=18): Batata(20), Tomate(8), Alho(10), Azeite(16)
(18, 20), (18, 8), (18, 10), (18, 16),
-- Quiche (id=19): Queijo(12), Presunto(13), Cebola(9)
(19, 12), (19, 13), (19, 9),
-- Torta de Frango (id=20): Frango(1), Queijo(12), Tomate(8), Cebola(9)
(20, 1), (20, 12), (20, 8), (20, 9),
-- === JANTAR (id 21-40) ===
-- Salmão (id=21): Peixe(3), Azeite(16), Limão(17), Alho(10)
(21, 3), (21, 16), (21, 17), (21, 10),
-- Camarão na Moranga (id=22): Camarão(4), Alho(10), Azeite(16), Cebola(9)
(22, 4), (22, 10), (22, 16), (22, 9),
-- Filé Mignon (id=23): Carne(2), Batata(20), Alho(10), Azeite(16)
(23, 2), (23, 20), (23, 10), (23, 16),
-- Risoto de Camarão (id=24): Camarão(4), Arroz(5), Tomate(8), Cream Cheese(18)
(24, 4), (24, 5), (24, 8), (24, 18),
-- Frango Recheado (id=25): Frango(1), Cream Cheese(18), Bacon(14), Arroz(5)
(25, 1), (25, 18), (25, 14), (25, 5),
-- Costela (id=26): Carne(2), Alho(10), Cebola(9), Batata(20)
(26, 2), (26, 10), (26, 9), (26, 20),
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
(40, 4), (40, 1), (40, 5), (40, 11), (40, 10),
-- === BEBIDAS ALMOÇO (id 41-55) ===
-- Suco de Laranja (id=41): Laranja(21), Açúcar(30)
(41, 21), (41, 30),
-- Suco de Maracujá (id=42): Maracujá(22), Açúcar(30)
(42, 22), (42, 30),
-- Suco de Morango (id=43): Morango(23), Açúcar(30)
(43, 23), (43, 30),
-- Suco de Abacaxi com Hortelã (id=44): Abacaxi(24), Hortelã(26)
(44, 24), (44, 26),
-- Suco de Manga (id=45): Manga(25), Açúcar(30)
(45, 25), (45, 30),
-- Limonada Suíça (id=46): Limão(17), Leite Condensado(35), Açúcar(30)
(46, 17), (46, 35), (46, 30),
-- Refrigerante (id=47): Açúcar(30)
(47, 30),
-- Água Mineral (id=48): (sem ingredientes cadastráveis)
-- Água com Gás (id=49): Água com Gás(34)
(49, 34),
-- Café Expresso (id=50): Café(29), Açúcar(30)
(50, 29), (50, 30),
-- Cappuccino (id=51): Café(29), Leite(28), Canela(33)
(51, 29), (51, 28), (51, 33),
-- Chá Gelado (id=52): Açúcar(30), Limão(17)
(52, 30), (52, 17),
-- Vitamina de Morango (id=53): Morango(23), Leite(28), Mel(32)
(53, 23), (53, 28), (53, 32),
-- Chocolate Quente (id=54): Cacau(31), Leite(28), Açúcar(30)
(54, 31), (54, 28), (54, 30),
-- Cerveja Pilsen (id=55): (sem ingredientes cadastráveis)
-- === BEBIDAS JANTAR (id 56-70) ===
-- Vinho Tinto (id=56): (sem ingredientes cadastráveis)
-- Vinho Branco (id=57): (sem ingredientes cadastráveis)
-- Espumante (id=58): (sem ingredientes cadastráveis)
-- Sangria (id=59): Laranja(21), Morango(23), Canela(33)
(59, 21), (59, 23), (59, 33),
-- Gin Tônica (id=60): Limão(17), Gengibre(27)
(60, 17), (60, 27),
-- Caipirinha de Limão (id=61): Limão(17), Açúcar(30)
(61, 17), (61, 30),
-- Caipirinha de Maracujá (id=62): Maracujá(22), Açúcar(30)
(62, 22), (62, 30),
-- Whisky (id=63): (sem ingredientes cadastráveis)
-- Mojito (id=64): Hortelã(26), Limão(17), Açúcar(30), Água com Gás(34)
(64, 26), (64, 17), (64, 30), (64, 34),
-- Aperol Spritz (id=65): Laranja(21), Água com Gás(34)
(65, 21), (65, 34),
-- Cerveja Artesanal (id=66): (sem ingredientes cadastráveis)
-- Suco de Laranja Jantar (id=67): Laranja(21), Açúcar(30)
(67, 21), (67, 30),
-- Água com Gás Jantar (id=68): Água com Gás(34)
(68, 34),
-- Café Expresso Duplo (id=69): Café(29), Açúcar(30)
(69, 29), (69, 30),
-- Digestivo Limoncello (id=70): Limão(17), Açúcar(30)
(70, 17), (70, 30);
GO

-- ============================================================
-- SUGESTOES DO CHEFE
-- Periodo: 1=Almoco, 2=Jantar
-- ============================================================
INSERT INTO SugestoesChefe (DataSugestao, Periodo, ItemCardapioId) VALUES
('2026-03-25', 1, 2),   -- Picanha no Almoço
('2026-03-25', 2, 22),  -- Camarão na Moranga no Jantar
('2026-03-26', 1, 1),   -- Frango Grelhado no Almoço
('2026-03-26', 2, 21),  -- Salmão no Jantar
('2026-03-27', 1, 4),   -- Macarronada no Almoço
('2026-03-27', 2, 23),  -- Filé Mignon no Jantar
('2026-03-28', 1, 3),   -- Peixe no Almoço
('2026-03-28', 2, 24),  -- Risoto de Camarão no Jantar
('2026-03-29', 1, 5),   -- Salada Caesar no Almoço
('2026-03-29', 2, 26),  -- Costela no Jantar
('2026-03-30', 1, 2),   -- Picanha no Almoço (hoje)
('2026-03-30', 2, 25);  -- Frango Recheado no Jantar (hoje)
GO

-- ============================================================
-- RESERVAS
-- StatusReserva: 1=Ativa, 2=Cancelada, 3=Finalizada, 4=Confirmada
-- ============================================================
SET IDENTITY_INSERT Reservas ON;
INSERT INTO Reservas (Id, DataHoraReserva, QuantidadePessoas, StatusReserva, CodigoConfirmacao, UsuarioID, MesaId) VALUES
(1,  '2026-03-30 11:30:00', 2, 4, 'CONF0001', 2, 1),
(2,  '2026-03-30 12:00:00', 4, 1, 'RESV0002', 3, 3),
(3,  '2026-03-30 12:30:00', 2, 4, 'CONF0003', 4, 2),
(4,  '2026-03-30 13:00:00', 6, 1, 'RESV0004', 5, 6),
(5,  '2026-03-29 12:00:00', 4, 3, 'CONF0005', 2, 4),
(6,  '2026-03-29 13:00:00', 2, 3, 'CONF0006', 3, 1),
(7,  '2026-03-28 11:30:00', 8, 3, 'CONF0007', 4, 8),
(8,  '2026-03-31 12:00:00', 4, 1, 'RESV0008', 5, 5),
(9,  '2026-03-31 13:00:00', 6, 1, 'RESV0009', 2, 7),
(10, '2026-04-01 12:00:00', 2, 1, 'RESV0010', 3, 2);
SET IDENTITY_INSERT Reservas OFF;
GO

-- ============================================================
-- ATENDIMENTOS
-- TipoAtendimento: 1=Presencial, 2=DeliveryProprio, 3=DeliveryAplicativo
-- Discriminator column (TPH inheritance)
-- ============================================================
SET IDENTITY_INSERT Atendimentos ON;
INSERT INTO Atendimentos (Id, TipoAtendimento, DataHora, TaxaEntrega, Discriminator, ObservacaoEntrega, NomeAplicativo) VALUES
(1,  1, '2026-03-25 12:30:00', 0.00,  'AtendimentoPresencial',       NULL,                   NULL),
(2,  1, '2026-03-25 13:00:00', 0.00,  'AtendimentoPresencial',       NULL,                   NULL),
(3,  1, '2026-03-25 20:00:00', 0.00,  'AtendimentoPresencial',       NULL,                   NULL),
(4,  2, '2026-03-26 12:15:00', 10.00, 'AtendimentoDeliveryProprio',  'Entregar no portão',   NULL),
(5,  2, '2026-03-26 19:45:00', 10.00, 'AtendimentoDeliveryProprio',  'Ligar ao chegar',      NULL),
(6,  3, '2026-03-27 12:00:00', 0.00,  'AtendimentoDeliveryAplicativo', NULL,                 'iFood'),
(7,  3, '2026-03-27 20:30:00', 0.00,  'AtendimentoDeliveryAplicativo', NULL,                 'Rappi'),
(8,  1, '2026-03-28 12:00:00', 0.00,  'AtendimentoPresencial',       NULL,                   NULL),
(9,  3, '2026-03-29 19:00:00', 0.00,  'AtendimentoDeliveryAplicativo', NULL,                 'iFood'),
(10, 2, '2026-03-30 12:30:00', 10.00, 'AtendimentoDeliveryProprio',  NULL,                   NULL);
SET IDENTITY_INSERT Atendimentos OFF;
GO

-- ============================================================
-- PEDIDOS
-- Periodo: 1=Almoco, 2=Jantar
-- ============================================================
SET IDENTITY_INSERT Pedidos ON;
INSERT INTO Pedidos (Id, DataHora, Periodo, Subtotal, Desconto, TaxaEntrega, Total, UsuarioId, AtendimentoId) VALUES
(1,  '2026-03-25 12:30:00', 1, 91.80,  0.00,  0.00,  91.80,  2, 1),
(2,  '2026-03-25 13:00:00', 1, 58.90,  0.00,  0.00,  58.90,  3, 2),
(3,  '2026-03-25 20:00:00', 2, 148.80, 0.00,  0.00,  148.80, 4, 3),
(4,  '2026-03-26 12:15:00', 1, 61.80,  0.00,  10.00, 71.80,  5, 4),
(5,  '2026-03-26 19:45:00', 2, 72.90,  0.00,  10.00, 82.90,  2, 5),
(6,  '2026-03-27 12:00:00', 1, 32.90,  0.00,  0.00,  32.90,  3, 6),
(7,  '2026-03-27 20:30:00', 2, 64.90,  0.00,  0.00,  64.90,  4, 7),
(8,  '2026-03-28 12:00:00', 1, 103.80, 5.00,  0.00,  98.80,  5, 8),
(9,  '2026-03-29 19:00:00', 2, 68.90,  0.00,  0.00,  68.90,  2, 9),
(10, '2026-03-30 12:30:00', 1, 86.40,  0.00,  10.00, 96.40,  3, 10);
SET IDENTITY_INSERT Pedidos OFF;
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
-- Pedido 3 (Pedro, Presencial, Jantar): Salmão(21) + Filé Mignon(23)
(3, 21, 1, 68.90, 68.90),
(3, 23, 1, 79.90, 79.90),
-- Pedido 4 (Mariana, DeliveryProprio, Almoço): Frango + Macarronada
(4, 1, 1, 32.90, 32.90),
(4, 4, 1, 28.90, 28.90),
-- Pedido 5 (Carlos, DeliveryProprio, Jantar): Camarão na Moranga(22)
(5, 22, 1, 72.90, 72.90),
-- Pedido 6 (Ana, iFood, Almoço): Frango
(6, 1, 1, 32.90, 32.90),
-- Pedido 7 (Pedro, Rappi, Jantar): Risoto de Camarão(24)
(7, 24, 1, 64.90, 64.90),
-- Pedido 8 (Mariana, Presencial, Almoço): 2x Frango + Picanha (desconto 5)
(8, 1, 2, 32.90, 65.80),
(8, 2, 1, 58.90, 58.90),  -- nota: subtotal pedido considera desconto no total, não no item
-- Pedido 9 (Carlos, iFood, Jantar): Salmão(21)
(9, 21, 1, 68.90, 68.90),
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
