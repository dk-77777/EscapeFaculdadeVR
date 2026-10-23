using System;
using TMPro;
using UnityEditor;
using UnityEditor.Events;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.UI;

namespace EscapeFaculdade.EditorTools
{
    public static class InitialPrototypeGenerator
    {
        private const string MenuPath = "Tools/Escape Faculdade/Gerar Protótipo Inicial";
        private const string GameFolder = "Assets/_EscapeFaculdade";
        private const string RigPath = "Assets/VRTemplateAssets/Prefabs/Setup/Complete XR Origin Set Up Hands Variant.prefab";
        private const float RoomWidth = 8f;
        private const float RoomDepth = 6f;
        private const float Height = 3f;
        private const float WallThickness = 0.2f;
        private const float DoorWidth = 0.9f;
        private const float DoorHeight = 2.1f;
        private const float CorridorWidth = 2f;
        private const float CorridorLength = 10f;

        [MenuItem(MenuPath)]
        public static void Generate()
        {
            if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
                return;

            try
            {
                string path = GeneratePrototype();
                Debug.Log("Protótipo inicial criado em " + path +
                    ". Em Play Mode, digite 4362 e pressione OK no keypad.");
            }
            catch (Exception exception)
            {
                Debug.LogException(exception);
                EditorUtility.DisplayDialog("Escape Faculdade", exception.Message, "OK");
            }
        }

        [MenuItem(MenuPath, true)]
        private static bool CanGenerate()
        {
            return !EditorApplication.isPlayingOrWillChangePlaymode && !EditorApplication.isCompiling;
        }

        // Também permite validar a geração em uma cópia do projeto via batch mode.
        public static string GeneratePrototype()
        {
            if (EditorApplication.isPlayingOrWillChangePlaymode)
                throw new InvalidOperationException("Saia do Play Mode antes de gerar o protótipo.");

            // Confere as dependências antes de trocar a cena aberta.
            GameObject rigPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(RigPath);
            if (rigPrefab == null)
                throw new InvalidOperationException("Prefab XR do VR Template não encontrado: " + RigPath);

            TMP_FontAsset font = TMP_Settings.defaultFontAsset;
            if (font == null)
                throw new InvalidOperationException("Importe TMP Essential Resources e configure a fonte padrão do TextMeshPro antes de gerar.");

            Shader shader = Shader.Find("Universal Render Pipeline/Lit");
            if (shader == null)
                throw new InvalidOperationException("Shader URP/Lit não encontrado. Confira a importação do URP existente.");

            EnsureFolder(GameFolder);
            EnsureFolder(GameFolder + "/Scenes");
            EnsureFolder(GameFolder + "/Materials");
            EnsureFolder(GameFolder + "/Materials/Graybox");

            Material floor = GetMaterial("Floor", 0.32f, shader);
            Material wall = GetMaterial("Wall", 0.68f, shader);
            Material door = GetMaterial("Door", 0.42f, shader);
            Material panel = GetMaterial("Panel", 0.08f, shader);
            Material key = GetMaterial("Key", 0.24f, shader);

            // Uma cena nova mantém as cenas originais do template intactas.
            Scene scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
            Transform root = Group("_EscapeFaculdade", null);
            Transform environment = Group("Environment", root);
            BuildClassroom(environment, floor, wall, panel, font);
            BuildCorridor(environment, floor, wall, font);

            Transform gameplay = Group("Gameplay", root);
            DoorController doorController = BuildDoor(gameplay, door);
            Transform xr = Group("XR", root);
            XRInteractionManager manager = BuildXR(xr, rigPrefab);
            BuildKeypad(gameplay, doorController, manager, panel, key, font);
            BuildLighting(Group("Lighting", root));

            string path = AssetDatabase.GenerateUniqueAssetPath(GameFolder + "/Scenes/PrototipoInicial.unity");
            if (!EditorSceneManager.SaveScene(scene, path))
                throw new InvalidOperationException("Não foi possível salvar a cena do protótipo. A cena continua aberta para recuperação.");

            Selection.activeGameObject = root.gameObject;
            return path;
        }

        private static void BuildClassroom(Transform environment, Material floor, Material wall,
            Material panel, TMP_FontAsset font)
        {
            Transform room = Group("Classroom", environment);
            Cube("Floor", room, new Vector3(0f, -WallThickness / 2f, 0f),
                new Vector3(RoomWidth, WallThickness, RoomDepth), floor);
            Cube("Ceiling", room, new Vector3(0f, Height + WallThickness / 2f, 0f),
                new Vector3(RoomWidth + 2f * WallThickness, WallThickness, RoomDepth + 2f * WallThickness), wall);

            Transform walls = Group("Walls", room);
            Cube("Back", walls, new Vector3(0f, Height / 2f, -RoomDepth / 2f - WallThickness / 2f),
                new Vector3(RoomWidth, Height, WallThickness), wall);
            Cube("Left", walls, new Vector3(-RoomWidth / 2f - WallThickness / 2f, Height / 2f, 0f),
                new Vector3(WallThickness, Height, RoomDepth + 2f * WallThickness), wall);
            Cube("Right", walls, new Vector3(RoomWidth / 2f + WallThickness / 2f, Height / 2f, 0f),
                new Vector3(WallThickness, Height, RoomDepth + 2f * WallThickness), wall);

            // Três blocos deixam um vão real: nenhum collider cobre a passagem.
            float sideWidth = (RoomWidth - DoorWidth) / 2f;
            float sideCenter = DoorWidth / 2f + sideWidth / 2f;
            float frontZ = RoomDepth / 2f + WallThickness / 2f;
            Cube("Front_Left", walls, new Vector3(-sideCenter, Height / 2f, frontZ),
                new Vector3(sideWidth, Height, WallThickness), wall);
            Cube("Front_Right", walls, new Vector3(sideCenter, Height / 2f, frontZ),
                new Vector3(sideWidth, Height, WallThickness), wall);
            Cube("Front_Lintel", walls, new Vector3(0f, DoorHeight + (Height - DoorHeight) / 2f, frontZ),
                new Vector3(DoorWidth, Height - DoorHeight, WallThickness), wall);
            Cube("Threshold", room, new Vector3(0f, -WallThickness / 2f, frontZ),
                new Vector3(DoorWidth, WallThickness, WallThickness), floor);

            Transform board = Group("PuzzleClue", room);
            board.localPosition = new Vector3(-2f, 1.7f, RoomDepth / 2f - 0.05f);
            Cube("Board", board, Vector3.zero, new Vector3(2.3f, 1.45f, 0.08f), panel);
            Label("Clue", board, new Vector3(0f, 0f, -0.045f), new Vector2(2.1f, 1.3f),
                "SALA DE AULA\n\n2 + 2 = ?\n5 - 2 = ?\n3 x 2 = ?\n8 / 4 = ?\n\nLeia de cima para baixo.", 0.12f, font);
        }

        private static void BuildCorridor(Transform environment, Material floor, Material wall, TMP_FontAsset font)
        {
            Transform corridor = Group("Corridor", environment);
            float startZ = RoomDepth / 2f + WallThickness;
            float centerZ = startZ + CorridorLength / 2f;
            Cube("Floor", corridor, new Vector3(0f, -WallThickness / 2f, centerZ),
                new Vector3(CorridorWidth, WallThickness, CorridorLength), floor);
            Cube("Ceiling", corridor, new Vector3(0f, Height + WallThickness / 2f, centerZ),
                new Vector3(CorridorWidth + 2f * WallThickness, WallThickness, CorridorLength), wall);
            Transform walls = Group("Walls", corridor);
            Cube("Left", walls, new Vector3(-CorridorWidth / 2f - WallThickness / 2f, Height / 2f, centerZ),
                new Vector3(WallThickness, Height, CorridorLength), wall);
            Cube("Right", walls, new Vector3(CorridorWidth / 2f + WallThickness / 2f, Height / 2f, centerZ),
                new Vector3(WallThickness, Height, CorridorLength), wall);
            Cube("End", walls, new Vector3(0f, Height / 2f, startZ + CorridorLength + WallThickness / 2f),
                new Vector3(CorridorWidth + 2f * WallThickness, Height, WallThickness), wall);
            Label("EndLabel", corridor, new Vector3(0f, 1.6f, startZ + CorridorLength - 0.02f),
                new Vector2(1.7f, 0.5f), "FIM DO PROTÓTIPO", 0.14f, font, Color.black);
        }

        private static DoorController BuildDoor(Transform gameplay, Material material)
        {
            Transform door = Group("Door_01", gameplay);
            door.localPosition = new Vector3(0f, 0f, RoomDepth / 2f + WallThickness);
            Transform pivot = Group("DoorPivot", door);
            pivot.localPosition = new Vector3(-DoorWidth / 2f, 0f, 0f);
            Cube("DoorMesh", pivot, new Vector3(DoorWidth / 2f, DoorHeight / 2f, 0f),
                new Vector3(DoorWidth, DoorHeight, 0.06f), material);
            Rigidbody body = pivot.gameObject.AddComponent<Rigidbody>();
            body.isKinematic = true;
            body.useGravity = false;

            DoorController controller = door.gameObject.AddComponent<DoorController>();
            SerializedObject settings = new SerializedObject(controller);
            settings.FindProperty("doorPivot").objectReferenceValue = pivot;
            // Abre em direção ao corredor, liberando o lado do keypad na sala.
            settings.FindProperty("openAngle").floatValue = -90f;
            settings.FindProperty("duration").floatValue = 0.7f;
            settings.FindProperty("startsOpen").boolValue = false;
            settings.ApplyModifiedPropertiesWithoutUndo();
            return controller;
        }

        private static XRInteractionManager BuildXR(Transform parent, GameObject rigPrefab)
        {
            GameObject rig = (GameObject)PrefabUtility.InstantiatePrefab(rigPrefab, parent);
            // Só a posição e orientação da instância mudam; câmera, inputs e locomoção são do template.
            rig.transform.localPosition = new Vector3(0f, 0.02f, -1.5f);
            rig.transform.localRotation = Quaternion.identity;
            PrefabUtility.RecordPrefabInstancePropertyModifications(rig.transform);

            XRInteractionManager manager = rig.GetComponentInChildren<XRInteractionManager>(true);
            if (manager == null)
                manager = Group("XR Interaction Manager", parent).gameObject.AddComponent<XRInteractionManager>();

            // Necessário para os canvases de controles já presentes no template.
            if (rig.GetComponentInChildren<EventSystem>(true) == null)
            {
                GameObject events = Group("EventSystem", parent).gameObject;
                events.AddComponent<EventSystem>();
                events.AddComponent<XRUIInputModule>();
            }

            return manager;
        }

        private static void BuildKeypad(Transform gameplay, DoorController door, XRInteractionManager manager,
            Material panelMaterial, Material keyMaterial, TMP_FontAsset font)
        {
            Transform root = Group("Keypad_01", gameplay);
            root.localPosition = new Vector3(1f, 1.35f, RoomDepth / 2f - 0.14f);
            Cube("Panel", root, Vector3.zero, new Vector3(0.68f, 0.94f, 0.08f), panelMaterial);
            KeypadController keypad = root.gameObject.AddComponent<KeypadController>();
            TMP_Text display = Label("Display", root, new Vector3(0f, 0.34f, -0.045f),
                new Vector2(0.58f, 0.13f), "----", 0.10f, font);
            SerializedObject settings = new SerializedObject(keypad);
            settings.FindProperty("correctCode").stringValue = "4362";
            settings.FindProperty("maxDigits").intValue = 4;
            settings.FindProperty("displayText").objectReferenceValue = display;
            settings.FindProperty("emptyText").stringValue = "----";
            settings.ApplyModifiedPropertiesWithoutUndo();
            if (keypad.onCorrectCode == null)
                keypad.onCorrectCode = new UnityEvent();
            UnityEventTools.AddPersistentListener(keypad.onCorrectCode, door.Open);
            EditorUtility.SetDirty(keypad);

            string[] labels = { "1", "2", "3", "4", "5", "6", "7", "8", "9", "LIM", "0", "OK" };
            for (int index = 0; index < labels.Length; index++)
            {
                string label = labels[index];
                KeypadButton.ButtonType type = label == "LIM" ? KeypadButton.ButtonType.Clear :
                    label == "OK" ? KeypadButton.ButtonType.Submit : KeypadButton.ButtonType.Digit;
                Vector3 position = new Vector3((index % 3 - 1) * 0.2f, 0.14f - (index / 3) * 0.17f, -0.07f);
                GameObject button = Cube("Button_" + label, root, position,
                    new Vector3(0.17f, 0.14f, 0.07f), keyMaterial);
                // O texto é irmão do cubo para manter uma escala de fonte uniforme.
                Label("Label_" + label, root, position + new Vector3(0f, 0f, -0.04f),
                    new Vector2(0.16f, 0.12f), label, label.Length > 1 ? 0.055f : 0.08f, font);
                KeypadButton key = button.AddComponent<KeypadButton>();
                SerializedObject keySettings = new SerializedObject(key);
                keySettings.FindProperty("keypad").objectReferenceValue = keypad;
                keySettings.FindProperty("buttonType").enumValueIndex = (int)type;
                keySettings.FindProperty("digit").stringValue = type == KeypadButton.ButtonType.Digit ? label : "";
                keySettings.ApplyModifiedPropertiesWithoutUndo();

                XRSimpleInteractable interactable = button.AddComponent<XRSimpleInteractable>();
                interactable.interactionManager = manager;
                interactable.colliders.Clear();
                interactable.colliders.Add(button.GetComponent<BoxCollider>());
                // Uma seleção chama Press uma vez; Activated não é ligado para evitar entrada dupla.
                UnityEventTools.AddVoidPersistentListener(interactable.selectEntered, key.Press);
                EditorUtility.SetDirty(interactable);
            }

            Label("Instructions", root, new Vector3(0f, -0.61f, -0.045f), new Vector2(1.25f, 0.20f),
                "Selecione os números e depois OK.\nLIM apaga a entrada.", 0.065f, font, Color.black);
        }

        private static void BuildLighting(Transform parent)
        {
            RenderSettings.skybox = null;
            RenderSettings.ambientMode = AmbientMode.Flat;
            RenderSettings.ambientLight = new Color(0.6f, 0.6f, 0.6f);
            RenderSettings.fog = false;
            // O URP de performance existente desativa luzes adicionais. Usamos só a luz principal.
            Light light = Group("GrayboxLight", parent).gameObject.AddComponent<Light>();
            light.transform.localRotation = Quaternion.Euler(50f, -30f, 0f);
            light.type = LightType.Directional;
            light.lightmapBakeType = LightmapBakeType.Realtime;
            light.color = Color.white;
            light.intensity = 1f;
            light.shadows = LightShadows.None;
            RenderSettings.sun = light;
        }

        private static Transform Group(string name, Transform parent)
        {
            Transform group = new GameObject(name).transform;
            group.SetParent(parent, false);
            return group;
        }

        private static GameObject Cube(string name, Transform parent, Vector3 position, Vector3 scale, Material material)
        {
            GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube.name = name;
            cube.transform.SetParent(parent, false);
            cube.transform.localPosition = position;
            cube.transform.localScale = scale;
            cube.GetComponent<Renderer>().sharedMaterial = material;
            return cube;
        }

        private static TMP_Text Label(string name, Transform parent, Vector3 position, Vector2 size,
            string content, float fontSize, TMP_FontAsset font, Color? color = null)
        {
            GameObject label = new GameObject(name, typeof(RectTransform), typeof(TextMeshPro));
            label.transform.SetParent(parent, false);
            label.transform.localPosition = position;
            TextMeshPro text = label.GetComponent<TextMeshPro>();
            text.font = font;
            text.text = content;
            text.fontSize = fontSize * 10f;
            text.alignment = TextAlignmentOptions.Center;
            text.color = color ?? Color.white;
            text.textWrappingMode = TextWrappingModes.NoWrap;
            text.rectTransform.sizeDelta = size;
            return text;
        }

        private static Material GetMaterial(string name, float gray, Shader shader)
        {
            string path = GameFolder + "/Materials/Graybox/" + name + ".mat";
            Material material = AssetDatabase.LoadAssetAtPath<Material>(path);
            if (material != null)
                return material;

            material = new Material(shader) { name = name };
            material.SetColor("_BaseColor", new Color(gray, gray, gray));
            material.SetFloat("_Smoothness", 0f);
            AssetDatabase.CreateAsset(material, path);
            return material;
        }

        private static void EnsureFolder(string path)
        {
            if (AssetDatabase.IsValidFolder(path))
                return;

            int separator = path.LastIndexOf('/');
            AssetDatabase.CreateFolder(path.Substring(0, separator), path.Substring(separator + 1));
        }
    }
}
