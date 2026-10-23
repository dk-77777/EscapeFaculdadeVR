# Puzzles planejados

## Puzzle 1 - Sala inicial
Objetivo: abrir a porta da sala.

No quadro:
- 2 + 2 = 4
- 5 - 2 = 3
- 3 x 2 = 6
- 8 / 4 = 2

Codigo: 4362

Implementacao: `KeypadController.cs` + `KeypadButton.cs` + `DoorController.cs`.

## Puzzle 2 - Laboratorio
Quatro pistas espalhadas pelo laboratorio:
- Monitor: 8
- Post-it: 2
- Quadro: 1
- Embaixo do teclado: 7

Codigo: 8217

Recompensa: Cartao 1.

Implementacao: keypad. No `onCorrectCode`, abrir gaveta e liberar o Cartao 1.

## Puzzle 3 - Biblioteca/sala de estudos
Objetos/livros:
- JAVA = 7
- REDES = 5
- BANCO DE DADOS = 2
- IA = 9

Pista de ordem: JAVA -> REDES -> BANCO DE DADOS -> IA.

Solucao: 7529.

Pode ser feito como keypad OU como puzzle fisico de sequencia usando `SequencePuzzle.cs`.

Recompensa: Cartao 2.

## Puzzle 4 - Sala do professor
O jogador encontra uma pista que leva ao computador do professor. Depois de resolver uma senha curta, um arquivo informa onde esta o Cartao 3.

Sugestao: senha baseada em uma referencia da disciplina, mas sem depender de conhecimento que o jogador nao recebeu dentro do jogo.

Recompensa: Cartao 3.

## Regra de design dos puzzles
Todo puzzle deve ter a resposta dentro do proprio jogo. Nunca obrigar o jogador a adivinhar informacao externa.
