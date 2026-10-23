# Orientações para agentes

## Localização e contexto

- A raiz do projeto Unity é `EscapeFaculdadeVR/`, relativa a este arquivo. Abra essa subpasta no Unity Hub e use-a como diretório de trabalho para operações do projeto.
- O jogo é um escape room VR ambientado em uma faculdade. O código próprio fica em `Assets/Scripts/`; cenas ficam em `Assets/Scenes/`.
- Consulte `ProjectSettings/ProjectVersion.txt` para a versão do Editor e `Packages/manifest.json` para as dependências. Os nomes de pastas em `Assets/Samples/` podem representar versões anteriores às dos pacotes instalados.
- Esta cópia não possui um repositório Git detectado na inicialização. Verifique a disponibilidade de Git antes de usar comandos de diff, commit ou branch.

## Documentação de design

Os caminhos abaixo são relativos à raiz do projeto Unity:

- Antes de alterar puzzles, pistas ou recompensas, leia `Docs/PUZZLES.md`. As respostas devem estar disponíveis dentro do jogo.
- Para narrativa, falas e eventos de perseguição, consulte `Docs/DIALOGOS_E_EVENTOS.md`; o documento contém sugestões, não comprovação de implementação.
- Para planejamento e critérios de entrega, consulte `Docs/CHECKLIST_DESENVOLVIMENTO.md`. Confirme o comportamento no projeto antes de marcar itens como concluídos.

## Alterações no Unity

- Siga o estilo dos scripts próximos: classes `MonoBehaviour` em arquivos de mesmo nome, indentação de quatro espaços, chaves em linhas separadas, campos privados configuráveis com `[SerializeField]` e eventos de integração via `UnityEvent`.
- Preserve nomes de campos serializados e métodos públicos usados pelo Inspector. Ao renomear, migre os dados serializados e verifique os listeners de eventos em cenas e prefabs.
- Preserve os arquivos `.meta` e seus GUIDs. Ao mover assets, mova seus `.meta` correspondentes; deixe o Unity gerar metadados de novos assets durante a importação.
- Faça mudanças de gameplay em `Assets/Scripts/`. Altere amostras importadas e `Assets/VRTemplateAssets/` somente quando a tarefa exigir mudanças nesses recursos.
- Trate `Library/`, `Logs/`, `Temp/`, `Obj/`, `UserSettings/`, arquivos `.csproj` e soluções geradas como saída do ambiente. Modifique os assets e configurações de origem.
- Em alterações de cenas e prefabs, mantenha as mudanças pontuais e confira referências de componentes, colisores e eventos após a importação.

## Validação e entrega

- Para alterações de C#, deixe o Unity importar e compilar os scripts e verifique o Console. Compilar uma solução gerada fora do Editor não valida a integração de cenas e XR.
- Teste em Play Mode o fluxo afetado, incluindo sucesso, erro e repetição quando aplicáveis. Para puzzles e cartões, confira também os eventos que liberam portas e a progressão do `GameManager`.
- Para mudanças de interação VR, confira o comportamento no headset quando disponível; diferencie testes no Editor de testes no dispositivo.
- Consulte `ProjectSettings/EditorBuildSettings.asset` e os perfis de build existentes antes de escolher cenas e plataforma. Não presuma que toda cena em `Assets/Scenes/` está incluída no build.
- Não foi encontrada uma suíte de testes própria na inicialização. Se adicionar testes de lógica, use o Unity Test Framework e execute a suíte pertinente no Editor.
- Na entrega, informe o que mudou, o que foi validado e quais verificações ficaram pendentes por falta do Editor ou headset. Mudanças apenas documentais exigem conferência de caminhos e conteúdo, sem build.
