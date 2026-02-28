# Fluxo de Desenvolvimento --- Aplicação Desktop de Cronometragem de Eventos Esportivos

## 1. Objetivo

Desenvolver uma aplicação desktop Windows para cronometragem de eventos
esportivos, com suporte a:

-   Cadastro e gerenciamento de atletas
-   Organização de eventos e campeonatos
-   Controle de categorias
-   Cronometragem precisa
-   Registro e persistência de resultados
-   Operação offline com banco local
-   Recurso para Envio de Resultados via API
-   Funcionamento com Equipamentos RFID

## 2. Stack Tecnológica

### Linguagem

-   C# (.NET 8 ou superior)

### Tipo de Aplicação

-   Windows Desktop Application
-   WPF

### Banco de Dados

-   MySQL

### ORM

-   Entity Framework Core

### Arquitetura

Application │ ├── UI (WPF) ├── Application Layer (Services) ├── Domain
Layer (Entities) ├── Infrastructure Layer (Database, Repositories) └──
Persistence Layer (EF Core)

## 3. Modelagem do Banco de Dados

### Usuario

Id, Nome, Login, SenhaHash, Tipo, Ativo, CreatedAt

### Categoria

Id, Descricao, Sexo, IdadeMin, IdadeMax, NaoClassificaGeral, Tipo (individual / duplas / trios / quartetps), CreatedAt

### Atleta

Id, NumeroDocumento, Nome, DataNascimento, Sexo, Equipe,Cidade, Estado,
CreatedAt

### Evento

Id, Nome, Data, Status, Modalidade, SepararGeral, QtGeral,
SepGeralMunicip, QtGeralMunicip, NomeMunicip, ClassificarTempoBruto,
HoraLargada, CreatedAt

### Evento_Trajetos

IdEvento, IdTrajeto, Nome, Distântia, QuantVoltas, HoraLargada,
CreatedAt

### Evento_Categorias

Id, IdEvento, IdTrajeto, IdCategoria, NroVoltas, HoraLargada

### Campeonato

Id, Nome, DataInicio, DataFim, Descricao, Pontuacao[1o a 15o colocado], PontuacaoParticipacao, PontuacaoOrganizador CreatedAt

### CampeonatoEventos

Id, IdCampeonato, IdEvento, PontuacaoEspecial(bool), Pontuacao[1o a 15o], PontuacaoParticipacao, PontuacaoOrganizador, CreatedAt

### InscricaoAtleta

Id, EventoId, EventoCategoriaId, Numero, TagRfid, Camisa, RetirouKit,
HoraLargada, Observacao, Organizador(bool), CreatedAt

### Chegadas

Id, AtletaId, EventoId, Volta, HoraChegada, FlagDNS, FlagDNF, FlagDSQ, CreatedAt

### Resultado

Id, AtletaId, EventoId, TempoBruto, TempoLiquido, TempoTotal, PosicaoGeral, PosicaoCategoria, Status, CreatedAt

## 4. Fluxo Operacional

Criar Campeonato (opcional) → Criar Evento → Criar Trajetos → Criar
Categorias → Vincular Trajetos e Categorias no Evento → Cadastrar
Atletas → Vincular Atletas Inscritos no Evento Iniciar Cronometragem
(registra hora de largada) → Registrar Tempos → Calcular Resultados →
Gerar Classificação

## 5. Fluxo de Cronometragem

Iniciar evento: Evento.Status = EmAndamento Evento.HoraInicio = Now()

Registrar largada: Resultado.TempoInicio = Timestamp atual

Registrar chegada: Resultado.TempoFim = Timestamp atual
Resultado.TempoTotal = TempoFim - TempoInicio

Classificação: ORDER BY TempoTotal ASC

## 6. Telas

Login\
Dashboard\
Gestão de Campeonatos\
Gestão de Eventos\
Gestão de Categorias\
Gestão de Atletas\
Tela de Cronometragem\
Resultados

## 7. Fluxo Técnico

1.  Criar solution .NET
2.  Criar projetos (UI, Domain, Application, Infrastructure)
3.  Criar entidades
4.  Criar DbContext
5.  Criar migrations
6.  Criar banco de dados
7.  Criar repositories
8.  Criar services
9.  Criar UI
10. Implementar lógica de cronometragem

## 8. Estado Final Esperado

Aplicação capaz de:

-   Criar campeonato
-   Criar evento
-   Cadastrar categorias
-   Cadastrar atletas
-   Cronometrar eventos
-   Registrar tempos
-   Calcular classificação
-   Exibir resultados
