
//====================================================================================================================================
//
// Copyright Julian Oliden "Jocyf" 2015 - 18-04-2015
// Procedural Cloud Texture v1.3
// Intensive particle system personalizable for Volumetric Clouds generation.
//
//====================================================================================================================================
using UnityEngine;
using UnityEditor;
using System.Collections;


[CustomEditor(typeof(CloudsToy))]
public class CloudsToyEditor : Editor {
	private int i = 0;
	private int MyWidth = 100;
	private bool showAdvancedSettings = false;
	private bool showShaderSettings = false;
	private bool showMaximunClouds = false;
	private CloudsToy CloudSystem;
	private ProceduralCloudTexture ProcText = null;
	private Color myColor;

    public override void OnInspectorGUI(){
		//EditorGUIUtility.LookLikeInspector();
		EditorGUIUtility.LookLikeControls();
		CloudSystem = (CloudsToy) target;

		if (!CloudSystem.gameObject) return; // If there isn't any cloudstoy gameobject in your scene, just return and do nothing at all.

		ProcText = (ProceduralCloudTexture) CloudSystem.ProceduralTexture;

		myColor = GUI.color;

		GUIStyle redFoldoutStyle = new GUIStyle(EditorStyles.foldout);
		redFoldoutStyle.normal.textColor = Color.red;
		redFoldoutStyle.focused.textColor = Color.red;
		redFoldoutStyle.hover.textColor = Color.red;
		redFoldoutStyle.active.textColor = Color.red;

		EditorGUILayout.BeginVertical();
		EditorGUILayout.Separator();
		EditorGUILayout.Separator();
		string[] MenuOptions = new string[2];
		MenuOptions[0] = "    Clouds    ";
		MenuOptions[1] = "Proc Texture";
		CloudSystem.ToolbarOptions = GUILayout.Toolbar(CloudSystem.ToolbarOptions, MenuOptions);
		EditorGUILayout.Separator();
		EditorGUILayout.Separator();
		EditorGUILayout.Separator();
		EditorGUILayout.EndVertical();
		EditorGUILayout.BeginVertical();
		switch (CloudSystem.ToolbarOptions){
			
			
		case 0: 
			// Is the CloudsToy being executed in Unity? If not, show the Maximun Clouds parameter.
			GUIContent contentFoldout = new GUIContent(" Maximun Clouds (DO NOT change while executing)", "Set the maximun clouds number that" +
			                                           "the CloudsToy system will handle. Changing this variable in runtime will crash the application.");
			if(!ProcText)
			{
				GUI.color = Color.red; 
				showMaximunClouds = EditorGUILayout.Foldout(showMaximunClouds, contentFoldout, redFoldoutStyle);

				if(showMaximunClouds)
					CloudSystem.MaximunClouds = EditorGUILayout.IntField("  ", CloudSystem.MaximunClouds);

				if (GUI.changed)
					EditorUtility.SetDirty(CloudSystem);
				GUI.color = myColor; 
				GUI.changed = false;
				EditorGUILayout.Separator();
				EditorGUILayout.Separator();
			}
			GUIContent contentCloud = new GUIContent("  Cloud Presets: ", "Cloud pressets to quickly start configuring the clouds style");
			CloudSystem.CloudPreset = (CloudsToy.TypePreset)EditorGUILayout.EnumPopup(contentCloud, CloudSystem.CloudPreset);
			if (GUI.changed)
			{
				if(CloudSystem.CloudPreset == CloudsToy.TypePreset.Stormy)
					CloudSystem.SetPresetStormy();
				else
				if(CloudSystem.CloudPreset == CloudsToy.TypePreset.Sunrise)
					CloudSystem.SetPresetSunrise();
				else
				if(CloudSystem.CloudPreset == CloudsToy.TypePreset.Fantasy)
					CloudSystem.SetPresetFantasy();
				EditorUtility.SetDirty(CloudSystem);
			}
			GUI.changed = false;
			
			EditorGUILayout.Separator();
			EditorGUILayout.Separator();
			contentCloud = new GUIContent("  Cloud Render: ", "Assign the shader that will be used to render the clouds particles.");
			CloudSystem.CloudRender = (CloudsToy.TypeRender)EditorGUILayout.EnumPopup(contentCloud, CloudSystem.CloudRender);
			contentCloud = new GUIContent("  Cloud Type: ", "Assign the texture that will be used to draw the clouds.");
			CloudSystem.TypeClouds = (CloudsToy.Type)EditorGUILayout.EnumPopup(contentCloud, CloudSystem.TypeClouds);
			contentCloud = new GUIContent("  Cloud Detail: ", "Cloud complexity that will created more populated clouds. " +
										  "Be aware that higher levels can drop your FPS drasticaly if there are a lot of clouds.");
			CloudSystem.CloudDetail = (CloudsToy.TypeDetail)EditorGUILayout.EnumPopup(contentCloud, CloudSystem.CloudDetail);
			if (GUI.changed)
			{
				CloudSystem.SetCloudDetailParams();
				EditorUtility.SetDirty(CloudSystem);
			}
			GUI.changed = false;
			EditorGUILayout.Separator();
			EditorGUILayout.Separator();

			GUI.color = Color.red; 
			contentFoldout = new GUIContent(" Particles Shader Settings", "You can change the shaders used in the clouds." +
				"It can be used, for example, to use different shaders in mobile applications.");
			showShaderSettings = EditorGUILayout.Foldout(showShaderSettings, contentFoldout, redFoldoutStyle);
			if(showShaderSettings)
			{
				EditorGUILayout.Separator();
				contentCloud = new GUIContent("Realistic Cloud Shader:", "Shader that will be used when selecting Realistic Clouds. This shader will use" +
					"the blended textures that can be assigned in the CloudsToy Texture paragraph. It is an alpha blended shader.");
				CloudSystem.realisticShader = (Shader)EditorGUILayout.ObjectField(contentCloud, CloudSystem.realisticShader, typeof(Shader), false);
				contentCloud = new GUIContent("Bright Cloud Shader:", "Shader that will be used when selecting Bright Clouds. This shader will use" +
				                              "the add textures that can be assigned in the CloudsToy Texture paragraph. It is an alpha additive shader.");
				CloudSystem.brightShader = (Shader)EditorGUILayout.ObjectField(contentCloud, CloudSystem.brightShader, typeof(Shader), false);
				contentCloud = new GUIContent("Projector Material:", "The projector material will be used to create the clouds shadows. " +
											  "By default it is usedthe Unity's default projector material.");
				CloudSystem.projectorMaterial = (Material)EditorGUILayout.ObjectField(contentCloud, CloudSystem.projectorMaterial, typeof(Material), false);
				EditorGUILayout.Separator();
			}
			EditorGUILayout.Separator();
			EditorGUILayout.Separator();

			contentFoldout = new GUIContent(" Particles Advanced Settings", "This section provides of two parameters to tweak your clouds. " +
				"It will be applied to all the cloud system. Those values can make your FPS drop drastically is you select very high values. Take it into account!");
			showAdvancedSettings = EditorGUILayout.Foldout(showAdvancedSettings, contentFoldout, redFoldoutStyle);
			if(showAdvancedSettings)
			{
				EditorGUILayout.Separator();
				contentCloud = new GUIContent("  Size Factor: ", "Modify the initial ellipsoid from wich the cloud particle is generated, so smaller (or bigger) clouds will be created.");
				CloudSystem.SizeFactorPart = EditorGUILayout.Slider(contentCloud, CloudSystem.SizeFactorPart, 0.1f, 4.0f);
				contentCloud = new GUIContent("  Emitter Mult: ", "Modify the minimun/maximun emission particle cloud, so more (or less) populated clouds will be created.");
				CloudSystem.EmissionMult = EditorGUILayout.Slider(contentCloud, CloudSystem.EmissionMult, 0.1f, 4.0f);
				EditorGUILayout.Separator();
			}
			GUI.color = myColor; 
			EditorGUILayout.Separator();
			EditorGUILayout.Separator();
			Rect buttonRect = EditorGUILayout.BeginHorizontal();
			buttonRect.x = buttonRect.width / 2 - 100;
			buttonRect.width = 200;
			buttonRect.height = 30;

			GUI.color = Color.red; 
			contentCloud = new GUIContent("Repaint Clouds", "Unity scene cloud regeneration and repainting. Use it when you want to be sure that all your tweaked adjustments are being applied to your clouds in the scene." +
			                              "It's ment to be used only in Unity while adjusting your clouds. DO NOT USE IT in your game in realtime execution; you will be recreating your clouds in your game just for nothing.");
			if(GUI.Button(buttonRect, contentCloud))
				CloudSystem.EditorRepaintClouds();
			GUI.color = myColor;

			EditorGUILayout.Separator();
			EditorGUILayout.EndHorizontal();
			EditorGUILayout.Separator();
			EditorGUILayout.Separator();
			EditorGUILayout.Separator();
			EditorGUILayout.Separator();
			EditorGUILayout.Separator();
			EditorGUILayout.Separator();
			contentCloud = new GUIContent("  Soft Clouds", "Modify particle render to stretch mode instead of the regular billboard mode.");
			CloudSystem.SoftClouds = EditorGUILayout.Toggle(contentCloud, CloudSystem.SoftClouds);
			if(CloudSystem.SoftClouds)
			{
				contentCloud = new GUIContent("  Spread Direction: ", "The world particle velocity that will be applied to the stretched clouds particles.");
				#if UNITY_4_0 || UNITY_4_0_1 || UNITY_4_1 || UNITY_4_2 || UNITY_4_3 || UNITY_4_5 || UNITY_4_6
				CloudSystem.SpreadDir = EditorGUILayout.Vector3Field("  Spread Direction: ", CloudSystem.SpreadDir);
				#else
				CloudSystem.SpreadDir = EditorGUILayout.Vector3Field(contentCloud, CloudSystem.SpreadDir);
				#endif
				contentCloud = new GUIContent("  Length Spread: ", "The scale lenght to which the clouds will be stretched to.");
				CloudSystem.LengthSpread = EditorGUILayout.Slider(contentCloud, CloudSystem.LengthSpread, 1, 30);
			}
			EditorGUILayout.Separator();
			contentCloud = new GUIContent("  Clouds Num: ", "Number of clouds that will be created. The maximum number of clouds that CloudsToy will handle" +
				"can be configured in the Maximum clouds parameter, the first cloudsToy parameter");
			CloudSystem.NumberClouds = EditorGUILayout.IntSlider(contentCloud, CloudSystem.NumberClouds, 1, CloudSystem.MaximunClouds);
			EditorGUILayout.Separator();
			contentCloud = new GUIContent("  Cloud Creation Size: ", "The scene blue box size where the clouds will be created into initially.");
			#if UNITY_4_0 || UNITY_4_0_1 || UNITY_4_1 || UNITY_4_2 || UNITY_4_3 || UNITY_4_5 || UNITY_4_6
			CloudSystem.Side = EditorGUILayout.Vector3Field("  Cloud Creation Size: ", CloudSystem.Side);
			#else
			CloudSystem.Side = EditorGUILayout.Vector3Field(contentCloud, CloudSystem.Side);
			#endif
			contentCloud = new GUIContent("  Dissapear Mult: ", "The scene yellow box will be calculated as a multiplier of the blue box. It is used" +
				"to know when to remove a cloud. So, when clouds are moving in any direction, as soon as they reach the yellow box border, they will be moved to the other side of the box");
			CloudSystem.DisappearMultiplier = EditorGUILayout.Slider(contentCloud, CloudSystem.DisappearMultiplier, 1, 10);
			EditorGUILayout.Separator();
			contentCloud = new GUIContent("  Maximum Velocity: ", "The clouds maximum velocity. Bigger clouds will be slower than the smaller ones.");
			#if UNITY_4_0 || UNITY_4_0_1 || UNITY_4_1 || UNITY_4_2 || UNITY_4_3 || UNITY_4_5 || UNITY_4_6
			CloudSystem.MaximunVelocity = EditorGUILayout.Vector3Field("  Maximun Velocity: ", CloudSystem.MaximunVelocity);
			#else
			CloudSystem.MaximunVelocity = EditorGUILayout.Vector3Field(contentCloud, CloudSystem.MaximunVelocity);
			#endif
			contentCloud = new GUIContent("  Velocity Mult: ", "A velocity multiplier to quickly tweak the clouds velocity without modifying the previous parameter.");
			CloudSystem.VelocityMultipier = EditorGUILayout.Slider(contentCloud, CloudSystem.VelocityMultipier, 0, 20);
			EditorGUILayout.Separator();
			contentCloud = new GUIContent("  Paint Type: ", "The clouds can be colorized with different adjustable colors using two diferent presets. Below paint type will be try to change the color of the" +
				"lower cloud particles to simulate how real clouds look like.");
			CloudSystem.PaintType = (CloudsToy.TypePaintDistr)EditorGUILayout.EnumPopup(contentCloud, CloudSystem.PaintType);
			/*if(CloudSystem.CloudRender == CloudsToy.TypeRender.Realistic)
			{*/
				contentCloud = new GUIContent("  Cloud Color: ", "Main cloud color used only in Realistic render mode. This color" +
					"will be directly assigned to the cloud realistic shader Tint color used by realistic particle clouds.");
				CloudSystem.CloudColor = EditorGUILayout.ColorField(contentCloud, CloudSystem.CloudColor);
			/*}*/
			contentCloud = new GUIContent("  Main Color: ", "This is the main color used when trying colorize the cloud.");
			CloudSystem.MainColor = EditorGUILayout.ColorField(contentCloud, CloudSystem.MainColor);
			contentCloud = new GUIContent("  Secondary Color: ", "This is the second color used when trying colorize the cloud.");
			CloudSystem.SecondColor = EditorGUILayout.ColorField(contentCloud, CloudSystem.SecondColor);
			contentCloud = new GUIContent("  Tint Strength: ", "Higher strenth will tint more cloud particles in the cloud.");
			CloudSystem.TintStrength = EditorGUILayout.IntSlider(contentCloud, CloudSystem.TintStrength, 1, 100);
			if(CloudSystem.PaintType == CloudsToy.TypePaintDistr.Below)
			{
				contentCloud = new GUIContent("  Offset: ", "Will be used in the below paint type to tint the cloud particles depending on" +
					"their relative position inside the cloud. Higher values will paint particles that are in high local positions inside the cloud");
				CloudSystem.offset = EditorGUILayout.Slider(contentCloud, CloudSystem.offset, 0, 1);
			}
			EditorGUILayout.Separator();
			contentCloud = new GUIContent("  Width: ", "Maximum width of each cloud");
			CloudSystem.MaxWithCloud = EditorGUILayout.IntSlider(contentCloud, CloudSystem.MaxWithCloud, 10, 1000);
			contentCloud = new GUIContent("  Height: ", "Maximum height of each cloud");
			CloudSystem.MaxTallCloud = EditorGUILayout.IntSlider(contentCloud, CloudSystem.MaxTallCloud, 5, 500);
			contentCloud = new GUIContent("  Depth: ", "Maximum depth of each cloud");
			CloudSystem.MaxDepthCloud = EditorGUILayout.IntSlider(contentCloud, CloudSystem.MaxDepthCloud, 5, 1000);
			contentCloud = new GUIContent("  Fixed Size", "The size of the clouds will be exactly the same depending on their cloud" +
				"size type (big, medium, small). If fixed size is disabled all the big clouds (for example) will not have the exact same size");
			CloudSystem.FixedSize = EditorGUILayout.Toggle(contentCloud, CloudSystem.FixedSize);
			EditorGUILayout.Separator();
			contentCloud = new GUIContent("  Animate Cloud", "Each cloud can be animated making it to rotate over itself.");
			CloudSystem.IsAnimate = EditorGUILayout.Toggle(contentCloud, CloudSystem.IsAnimate);
			if(CloudSystem.IsAnimate)
			{
				contentCloud = new GUIContent("  Animation Velocity: ", "Cloud rotation velocity.");
				CloudSystem.AnimationVelocity = EditorGUILayout.Slider(contentCloud, CloudSystem.AnimationVelocity, 0, 1);
			}
			contentCloud = new GUIContent("  Shadows: ", "Clouds can have a shadow. It is made using a Unity's projector that creates a shadow" +
				"taking into account the layer the clouds are in (so they will ignore the cloud particle itself when drawing their own shadow");
			CloudSystem.NumberOfShadows = (CloudsToy.TypeShadow)EditorGUILayout.EnumPopup(contentCloud, CloudSystem.NumberOfShadows);
			EditorGUILayout.Separator();
			EditorGUILayout.Separator();

			contentCloud = new GUIContent("Texture Add", "These are the textures that will be used by bright type clouds. Bright clouds " +
				"use a particle additive kind of shader.");
			GUIContent contentCloud2 = new GUIContent("(Used for Bright Clouds)", "");
			EditorGUILayout.LabelField(contentCloud, contentCloud2);
			EditorGUILayout.BeginHorizontal();
			for(i = 0; i < CloudSystem.CloudsTextAdd.Length; i++)
			{
				if(i == CloudSystem.CloudsTextAdd.Length/2)
				{
					EditorGUILayout.EndHorizontal();
					EditorGUILayout.Separator();
					EditorGUILayout.BeginHorizontal();
				}
				else
				if(i != 0 && i != 3)
					EditorGUILayout.Separator();
				CloudSystem.CloudsTextAdd[i] = (Texture2D)EditorGUILayout.ObjectField(CloudSystem.CloudsTextAdd[i], typeof(Texture2D), false, GUILayout.Width(70), GUILayout.Height(70));
			}
			EditorGUILayout.EndHorizontal();
			EditorGUILayout.Separator();
			EditorGUILayout.Separator();

			contentCloud = new GUIContent("Texture Blended", "These are the textures that will be used by realistic type clouds. Bright clouds " +
			                              "use a particle blended additive kind of shader.");
			contentCloud2 = new GUIContent("(Used for Realistic Clouds)", "");
			EditorGUILayout.LabelField(contentCloud, contentCloud2);
			EditorGUILayout.BeginHorizontal();
			for(i = 0; i < CloudSystem.CloudsTextBlended.Length; i++)
			{
				if(i == CloudSystem.CloudsTextBlended.Length/2)
				{
					EditorGUILayout.EndHorizontal();
					EditorGUILayout.Separator();
					EditorGUILayout.BeginHorizontal();
				}
				else
				if(i != 0 && i != 3)
					EditorGUILayout.Separator();
				CloudSystem.CloudsTextBlended[i] = (Texture2D)EditorGUILayout.ObjectField(CloudSystem.CloudsTextBlended[i], typeof(Texture2D), false, GUILayout.Width(70), GUILayout.Height(70));
			}
			EditorGUILayout.EndHorizontal();
			EditorGUILayout.Separator();
			if (GUI.changed)
				EditorUtility.SetDirty (CloudSystem);
			GUI.changed = false;
			break;
			
			
		case 1:
			if(!ProcText)
			{
				GUI.color = Color.red; 
				contentCloud = new GUIContent("  Texture Width: ", "Texture width used to create the runtime noise based texture. Because the texture is generated at runtime, " +
					"big texture sizes will slow the process. The texture is generated pixel by pixel so an 256x256 texture will be FOUR TIMES SLOWER than a 128x128 texture");
				CloudSystem.PT1TextureWidth = EditorGUILayout.IntField(contentCloud, CloudSystem.PT1TextureWidth);
				contentCloud = new GUIContent("  Texture Height: ", "Texture height used to create the runtime noise based texture. Because the texture is generated at runtime, " +
				                              "big texture sizes will slow the process. The texture is generated pixel by pixel so an 256x256 texture will be FOUR TIMES SLOWER than a 128x128 texture");
				CloudSystem.PT1TextureHeight = EditorGUILayout.IntField(contentCloud, CloudSystem.PT1TextureHeight);
				if (GUI.changed && ProcText)
					EditorUtility.SetDirty(CloudSystem);
				GUI.color = myColor; 
			}
			GUI.changed = false;
			EditorGUILayout.Separator();
			contentCloud = new GUIContent("  Type of Noise: ", "There are two different noise generator algorithms: The standard noise generation (Cloud) and the usual perlin noise generator.");
			CloudSystem.PT1TypeNoise = (CloudsToy.NoisePresetPT1)EditorGUILayout.EnumPopup(contentCloud, CloudSystem.PT1TypeNoise);
			EditorGUILayout.Separator();
			contentCloud = new GUIContent("  Seed: ", "This value is the basic seed value to generate any ramdom noise Diferent values will generate different noise patterns.");
			CloudSystem.PT1Seed = EditorGUILayout.IntSlider(contentCloud, CloudSystem.PT1Seed, 1, 10000);
			EditorGUILayout.Separator();
			if (GUI.changed && ProcText)
			{
				CloudSystem.PT1NewRandomSeed();
				EditorUtility.SetDirty(CloudSystem);
			}
			GUI.changed = false;

			contentCloud = new GUIContent("  Scale Width: ", "Internal noise scale width used when generating the noise pattern.");
			CloudSystem.PT1ScaleWidth = EditorGUILayout.Slider(contentCloud, CloudSystem.PT1ScaleWidth, 0.1f, 50.0f);
			contentCloud = new GUIContent("  Scale Height: ", "Internal noise scale height used when generating the noise pattern.");
			CloudSystem.PT1ScaleHeight = EditorGUILayout.Slider(contentCloud, CloudSystem.PT1ScaleHeight, 0.1f, 50.0f);
			contentCloud = new GUIContent("  Scale Factor: ", "Scale multiplier used to quickly tweak the scale width/height at once.");
			CloudSystem.PT1ScaleFactor = EditorGUILayout.Slider(contentCloud, CloudSystem.PT1ScaleFactor, 0.1f, 2.0f);
			EditorGUILayout.Separator();
			if(CloudSystem.PT1TypeNoise == CloudsToy.NoisePresetPT1.PerlinCloud)
			{
				contentCloud = new GUIContent("  Lacunarity: ", "Lacunarity parameter used by Perlin noise generator.");
				CloudSystem.PT1Lacunarity = EditorGUILayout.Slider(contentCloud, CloudSystem.PT1Lacunarity, 0.0f, 10.0f);
				contentCloud = new GUIContent("  FractalIncrement: ", "FractalIncrement parameter used by Perlin noise generator.");
				CloudSystem.PT1FractalIncrement = EditorGUILayout.Slider(contentCloud, CloudSystem.PT1FractalIncrement, 0.0f, 2.0f);
				contentCloud = new GUIContent("  Octaves: ", "Octave parameter used by Perlin noise generator.");
				CloudSystem.PT1Octaves = EditorGUILayout.Slider(contentCloud, CloudSystem.PT1Octaves, 0.0f, 10.0f);
				contentCloud = new GUIContent("  Offset: ", "Offset parameter used by Perlin noise generator (HybridMultifractal noise functions).");
				CloudSystem.PT1Offset = EditorGUILayout.Slider(contentCloud, CloudSystem.PT1Offset, 0.1f, 3.0f);
			}else
			if(CloudSystem.PT1TypeNoise == CloudsToy.NoisePresetPT1.Cloud)
			{
				contentCloud = new GUIContent("  Turb Size: ", "Turbulence parameter used by SimpleNoise Turbulence generator. Internally, this is the octaves parameter" +
					"used to calculate the turbulence of an already created SimpleNoise texture.");
				CloudSystem.PT1TurbSize = EditorGUILayout.IntSlider(contentCloud, CloudSystem.PT1TurbSize, 1, 256);
				contentCloud = new GUIContent("  Turb Lacun: ", "Lacunarity parameter used by SimpleNoise Turbulence generator. Internally, this is the lacunarity parameter" +
				                              "used to calculate the turbulence of an already created SimpleNoise texture.");
				CloudSystem.PT1TurbLacun = EditorGUILayout.Slider(contentCloud, CloudSystem.PT1TurbLacun, 0.01f, 0.99f);
				contentCloud = new GUIContent("  Turb Gain: ", "Gain parameter used by SimpleNoise Turbulence generator. Internally, this is the gain parameter" +
				                              "used to calculate the turbulence of an already created SimpleNoise texture. Higher values will generate brighter textures.");
				CloudSystem.PT1TurbGain = EditorGUILayout.Slider(contentCloud, CloudSystem.PT1TurbGain, 0.01f, 2.99f);
				contentCloud = new GUIContent("  Radius: ", "Used to adjust the noise turbulence. Lower values will generate darker textures because the resulting texture" +
					"will dark the pixels outside that radious.");
				CloudSystem.PT1xyPeriod = EditorGUILayout.Slider(contentCloud, CloudSystem.PT1xyPeriod, 0.1f, 2.0f);
				contentCloud = new GUIContent("  Turb Power: ", "Turbulence multipler that will affect the pixels inside the turbulence radious. " +
					"Higher values will generate brighter results BUT it will only affect the pixels inside a given Radious.");
				CloudSystem.PT1turbPower = EditorGUILayout.Slider(contentCloud, CloudSystem.PT1turbPower, 1, 60);
			}
			EditorGUILayout.Separator();
			/*CloudSystem.PT1IsHalo = EditorGUILayout.Toggle("  Halo Active:", CloudSystem.PT1IsHalo);
			if(CloudSystem.PT1IsHalo)*/
			contentCloud = new GUIContent("  HaloEffect: ", "Will create a dark halo around the texture, used to make rounded textures that can be used to draw the clouds.");
			CloudSystem.PT1HaloEffect = EditorGUILayout.Slider(contentCloud, CloudSystem.PT1HaloEffect, 0.1f, 1.7f);
			contentCloud = new GUIContent("  Inside Radius: ", "Will dark the pixels inside the Halo, used to teawk rounded textures that can be used to draw the clouds.");
			CloudSystem.PT1HaloInsideRadius = EditorGUILayout.Slider(contentCloud, CloudSystem.PT1HaloInsideRadius, 0.1f, 3.5f);
			EditorGUILayout.Separator();
			/*CloudSystem.PT1BackgroundColor = EditorGUILayout.ColorField("  Back Color: ", CloudSystem.PT1BackgroundColor);
			CloudSystem.PT1FinalColor = EditorGUILayout.ColorField("  Front Color: ", CloudSystem.PT1FinalColor);*/
			contentCloud = new GUIContent("  Invert Colors:", "Invert the texture colors.");
			CloudSystem.PT1InvertColors = EditorGUILayout.Toggle(contentCloud, CloudSystem.PT1InvertColors);
			contentCloud = new GUIContent("  Contrast Mult: ", "Higher values will create brighter textures.");
			CloudSystem.PT1ContrastMult = EditorGUILayout.Slider(contentCloud, CloudSystem.PT1ContrastMult, 0.0f, 2.0f);
			EditorGUILayout.Separator();
			contentCloud = new GUIContent("  Alpha Texture:", "It will create a second texture with transparency so the alpha channel can be tweaked." +
				"This new alpha texture will be draw in the inspector so you can see the alpha channel (the alpha values will be shown in green color.");
			CloudSystem.PT1UseAlphaTexture = EditorGUILayout.Toggle(contentCloud, CloudSystem.PT1UseAlphaTexture);
			if(CloudSystem.PT1UseAlphaTexture)
			{
				contentCloud = new GUIContent("  Alpha Index: ", "This value 0-1 will be used to increase/decrease the texture's alpha channel.");
				CloudSystem.PT1AlphaIndex = EditorGUILayout.Slider(contentCloud, CloudSystem.PT1AlphaIndex, 0.0f, 1.0f);
			}
			EditorGUILayout.Separator();
			EditorGUILayout.Separator();
			
			// Be sure that the Texture1 class exists before try to paint the textures.
			if(ProcText)
			{
				contentCloud = new GUIContent("  InEditor Text Size ", "Texture size used only in the inspector window.");
				MyWidth = EditorGUILayout.IntSlider(contentCloud, MyWidth, 50, 200);
				EditorGUILayout.Separator();
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.Space();
				ProcText.MyTexture = (Texture2D)EditorGUILayout.ObjectField(ProcText.MyTexture, typeof(Texture2D), false, GUILayout.Width(MyWidth), GUILayout.Height(MyWidth));
				EditorGUILayout.Separator();
				EditorGUILayout.Space();
				if(CloudSystem.PT1UseAlphaTexture)
					ProcText.MyAlphaDrawTexture = (Texture2D)EditorGUILayout.ObjectField(ProcText.MyAlphaDrawTexture, typeof(Texture2D), false, GUILayout.Width(MyWidth), GUILayout.Height(MyWidth));
				EditorGUILayout.Separator();
				EditorGUILayout.EndHorizontal();
				EditorGUILayout.Separator();
				EditorGUILayout.Separator();
				EditorGUILayout.Separator();
			}
			
			Rect buttonRectPT1 = EditorGUILayout.BeginHorizontal();
			buttonRectPT1.x = buttonRectPT1.width / 2 - 100;
			buttonRectPT1.width = 200;
			buttonRectPT1.height = 30;
			//GUI.skin??
			GUI.color = Color.red; 
			contentCloud = new GUIContent("Reset Parameters", "Reset the noise parameters to their default values.");
			if(GUI.Button(buttonRectPT1, contentCloud))
			{
				CloudSystem.ResetCloudParameters();
				if(ProcText)
					CloudSystem.PT1CopyParameters();
			}
			EditorGUILayout.EndHorizontal();
			EditorGUILayout.Separator();
			EditorGUILayout.Separator();
			EditorGUILayout.Separator();
			EditorGUILayout.Separator();
			EditorGUILayout.Separator();
			EditorGUILayout.Separator();
			EditorGUILayout.Separator();
			EditorGUILayout.Separator();
			// Is the program being executed? If so, show the 'Save Params' button.
			GUI.color = Color.yellow;
			if(ProcText)
			{
				Rect buttonRectPrint = EditorGUILayout.BeginHorizontal();
				buttonRectPrint.x = buttonRectPrint.width / 2 - 100;
				buttonRectPrint.width = 200;
				buttonRectPrint.height = 30;
				//GUI.skin??
				contentCloud = new GUIContent("Save Texture", "Save the generated texture to a file. CAUTION: This funcion can not be used in Web Player targeted projects.");
				if(GUI.Button(buttonRectPrint, contentCloud))
					CloudSystem.SaveProceduralTexture();
				EditorGUILayout.EndHorizontal();
			}

			GUI.color = myColor; 
			EditorGUILayout.Separator();
			EditorGUILayout.Separator();
			EditorGUILayout.Separator();
			EditorGUILayout.Separator();
			EditorGUILayout.Separator();
			EditorGUILayout.Separator();
			EditorGUILayout.Separator();
			EditorGUILayout.Separator();

			if (GUI.changed)
			{
				if(ProcText)
				{
					CloudSystem.PT1CopyParameters();
					CloudSystem.ModifyPTMaterials();
				}
				EditorUtility.SetDirty (CloudSystem);
			}
			GUI.changed = false;
			
			break;
		
		}
		EditorGUILayout.EndVertical();
    }
}
