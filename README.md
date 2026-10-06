# Relicbound

Um pequeno projeto experimental de Unity criado para estudar arquitetura de sistemas e geração procedural em um jogo 2D.

> **Status:** Projeto encerrado como experimento / estudo.

O objetivo nunca foi transformar este projeto em um jogo completo. A ideia era construir sistemas suficientes para observar como diferentes partes de um jogo podem ser separadas, compostas e conectadas de forma desacoplada.

## Objetivos

- Arquitetura de sistemas em Unity.
- Separação de responsabilidades.
- Sistema de relics baseado em composição.
- Progressão persistente do jogador.
- Geração procedural determinística.
- Geração de salas e grafos.
- Geração procedural de plataformas.
- Validação de alcance do movimento.
- Grapple e swing.
- Hazards e recompensas procedurais.
- Transição entre salas.
- Geração de novos mundos através de novas seeds.
- Save/load da progressão.

## Tecnologias

- Unity 6000.5.4f1
- C#
- Unity Input System
- Cinemachine
- ScriptableObjects
- JSON para save
- `System.Random` para geração determinística

## Gameplay

O loop principal do protótipo:

```text
Explorar
   ↓
Encontrar relics
   ↓
Equipar habilidades
   ↓
Usar novas capacidades de movimento
   ↓
Superar obstáculos
   ↓
Encontrar a saída
   ↓
Gerar um novo mundo
   ↓
Continuar com a progressão
```

O jogador mantém sua progressão enquanto o mundo é reconstruído a partir de uma nova seed.

## Sistemas implementados

### Movimento

- Movimento horizontal.                      a/d
- Pulo.                                      space
- Dash.                                      left shift
- Double jump.                               space
- Modificadores de velocidade.               a/d   
- Modificadores de altura do pulo.           space
- Dash direcional.                           w/s/wa/sa/wd/sd + left shift
- Dash limitado por disponibilidade.         left shift
- Salvar estado do jogador.                  F5
- Carregar estado do jogador.                F9 
- grapple.                                   e  

O projeto utiliza exclusivamente o **novo Unity Input System**.

### Grapple

O sistema de grapple permite:

- Detectar pontos próximos.
- Selecionar pontos por direção e distância.
- Verificar linha de visão.
- Conectar o jogador ao ponto.                  
- Manter uma distância da âncora.
- Movimento pendular.
- Preservação de momentum.
- Lançamento ao soltar.
- Seleção direcional de alvos.

Os grapple points também podem ser gerados proceduralmente.

### Relics

Os relics concedem capacidades ao jogador.

Exemplos implementados:

- Double Jump
- High Jump
- Speed
- Extra Dash
- Long Dash
- Upward Dash

A arquitetura separa relics das capabilities:

```text
Relic
   ↓
Capability
   ↓
Player
```

Cada relic possui sua própria lógica de aplicação e remoção.

### Inventory e Equipment

O jogador possui:

- Inventário de relics.
- Slots de equipamento.
- Equipar/unequip.
- Relics duplicados.
- Persistência dos relics equipados.
- Expansão permanente da quantidade de slots.

A expansão de slots é tratada como **progressão do jogador**, e não como um relic equipado.

### Mundo procedural

O mundo é construído através de um pipeline:

```text
Seed
 ↓
WorldGenerator
 ↓
WorldRoomGraph
 ↓
WorldLayoutGenerator
 ↓
WorldBuilder
 ↓
ProceduralRoom
 ↓
Platforms
Grapples
Hazards
Rewards
```

A geração utiliza seeds para produzir mundos determinísticos.

### Room Graph

O mundo é representado primeiro como um grafo abstrato antes de virar GameObjects.

As salas possuem:

- ID.
- Tipo.
- Conexões.

O protótipo explorou tipos como:

- Start
- Exploration
- Reward
- Challenge
- Exit

A intenção foi separar a **estrutura do mundo** da **representação física da sala**.

### Procedural Platforms

As plataformas são geradas respeitando regras de alcance:

- Distância mínima.
- Alcance horizontal.
- Alcance vertical.
- Dash.
- Grapple.
- Caminho principal.
- Branches.
- Dead ends.
- Start e Exit.

A geração passou por várias iterações porque gerar plataformas aleatoriamente não é suficiente: elas precisam ser interessantes e navegáveis.

### Procedural Grapple

O gerador considera:

- Distância entre plataformas.
- Distância do ponto em relação às plataformas.
- Espaço disponível.
- Movimento vertical.
- Movimento horizontal.
- Diferença de rota.
- Limite de conexões.

O resultado são grapple points posicionados no espaço entre plataformas.

### Hazards

Foram adicionados hazards simples gerados sobre plataformas.

O sistema considera:

- Plataforma válida.
- Distância das bordas.
- Limite de hazards por sala.
- Exclusão de algumas plataformas inadequadas.

### Procedural Rewards

As recompensas também fazem parte da geração procedural.

Tipos implementados:

- Relic.
- Expansão de slot de relic.

A geração separa:

```text
onde colocar a recompensa
```

de:

```text
qual objeto concreto representa a recompensa
```

### Transição entre salas

As salas possuem conexões geradas pelo grafo.

Ao atravessar uma saída:

```text
Room A
  ↓
Room B
```

o jogador é movido para a posição de entrada da próxima sala e seu estado de movimento é resetado.

### Transição entre mundos

Ao alcançar a saída final:

```text
Exit Room
    ↓
World Exit
    ↓
Nova Seed
    ↓
Novo mundo
```

O mundo atual é reconstruído com uma nova seed.

A progressão do jogador permanece:

```text
Relics
Slots
Capabilities
```

Enquanto o mundo muda:

```text
Rooms
Platforms
Grapples
Hazards
Rewards
```

## Save / Load

O projeto possui persistência em JSON.

São persistidos dados como:

- Seed do mundo.
- Posição do jogador.
- Relics do inventário.
- Relics equipados.
- Quantidade de slots.

Os relics são reconstruídos em runtime a partir dos IDs salvos.

## Arquitetura

Uma das principais coisas estudadas foi a separação entre **dados, regras e representação**.

Exemplo do sistema de relics:

```text
RelicData
    ↓
Relic
    ↓
RelicManager
    ↓
RelicEquipment
    ↓
PlayerCapabilityController
    ↓
PlayerMovement
```

E do mundo procedural:

```text
WorldGenerator
    ↓
WorldRoomGraph
    ↓
WorldLayout
    ↓
WorldBuilder
    ↓
ProceduralRoom
    ├── PlatformGenerator
    ├── GrappleConnectionGenerator
    ├── HazardGenerator
    └── RewardGenerator
```

A intenção foi evitar que um único componente conhecesse todos os detalhes do sistema.

## O que este projeto ensinou

A principal lição foi que **geração procedural é consideravelmente mais difícil do que simplesmente gerar coisas aleatórias**.

Gerar plataformas é relativamente fácil.

Gerar plataformas que:

- sejam alcançáveis;
- tenham variedade;
- formem caminhos interessantes;
- permitam diferentes estilos de movimento;
- criem branches;
- ofereçam boas posições para grapple;
- acomodem hazards;
- tenham recompensas em locais interessantes;
- funcionem com diferentes seeds;
- mantenham uma progressão coerente;

é um problema muito maior.

O projeto mostrou na prática a diferença entre:

```text
Randomização
```

e:

```text
Geração procedural com intenção.
```

Também mostrou por que a arquitetura precisa acompanhar essa complexidade. Conforme o mundo ganhou plataformas, grapple, hazards, recompensas e tipos de sala, ficou cada vez mais importante separar:

```text
Graph
Layout
Generation
Validation
Building
Gameplay
```

## Por que o projeto foi encerrado

O objetivo principal era aprendizado, não produção.

Depois de implementar:

- movimento;
- grapple;
- relics;
- inventory;
- equipment;
- capabilities;
- save/load;
- procedural rooms;
- procedural platforms;
- procedural grapple;
- hazards;
- rewards;
- room transitions;
- world transitions;

o projeto já havia cumprido seu propósito.

Continuar adicionando conteúdo seria possível, mas começaria a transformar um laboratório de arquitetura em um projeto de jogo propriamente dito.

Por isso, o projeto foi encerrado neste ponto.

## Possíveis próximos passos

Caso fosse retomado:

- Melhorar a classificação e intenção das salas.
- Templates diferentes para cada tipo.
- Salas de desafio mais elaboradas.
- Balanceamento procedural.
- Regras de dificuldade baseadas na progressão.
- Melhor geração de recompensas.
- Mais tipos de hazards.
- Inimigos simples.
- Combinações de relics.
- Visualização do grafo procedural.
- Ferramentas de debug para analisar seeds.
- Geração orientada por objetivos.

Nenhum desses itens é necessário para considerar o experimento concluído.

## Conclusão

**Relicbound foi um laboratório de arquitetura e geração procedural em Unity.**

O valor principal não está em ser um jogo completo, mas em permitir experimentar a construção de vários sistemas interdependentes e observar onde a complexidade realmente aparece.

O projeto cumpriu seu objetivo:

> entender melhor como sistemas de gameplay podem ser compostos e, principalmente, perceber na prática por que geração procedural de qualidade exige muito mais do que aleatoriedade.
