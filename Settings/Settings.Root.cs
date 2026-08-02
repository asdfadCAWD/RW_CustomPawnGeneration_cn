using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RW_CustomPawnGeneration
{
	public partial class Settings
	{
		public const string DESCRIPTION_ADVANCED_MODE =
			"显示每个种族的单独设置。" +
			"禁用此选项不会阻止种族特定选项" +
			"（参见\"使用种族特定选项\"。）";
		public const string DESCRIPTION_RESET_ALL =
			"这将恢复所有种族设置的所有默认值" +
			"（包括[全局配置]），且无法撤销。" +
			"确定要这样做吗？";
		public const string DESCRIPTION_USE_RACE_SPECIFIC =
			"启用后，将使用种族特定选项" +
			"（禁用\"高级设置\"不会阻止此功能。）";
		public const string DESCRIPTION_CUSTOM_AGING =
			"启用后，允许角色拥有自定义的年龄增长速度（以游戏刻为单位）。" +
			"此选项可能会显著降低游戏性能，特别是在规模较大的殖民地中。" +
			"不影响其他年龄相关选项。\n" +
			"需要重启游戏才能生效。";
		public const string DESCRIPTION_GLOBAL_CONFIG =
			"所有未修改任何设置或" +
			"使用了[使用全局配置]选项的种族将引用此配置。" +
			"部分选项仅在必要时才会生效（如体型仅适用于类人种族等）。";
		public const string DESCRIPTION_UNGENDERED_PARENT =
			"启用后，所有角色无论性别都可以成为母亲或父亲。" +
			"\n有玩家反馈这可能导致卡顿，" +
			"可能是模组不兼容导致。" +
			"\n如果这对你造成了卡顿，请禁用此选项。" +
			"\n需要重启游戏才能生效。";

		public const string RESET = "重置";
		public const string RESET_ALL = "全部重置";
		public const string YES = "是";
		public const string NO = "否";
		public const string COPY_TO = "复制到...";
		public const string EDIT = "编辑";	
		public const string SHOW_CONFIG = "显示配置";
		public const string ADVANCED_MODE = "高级设置";
		public const string USE_RACE_SPECIFIC = "使用种族特定选项";
		public const string CUSTOM_AGING = "启用自定义年龄刻";
		public const string UNGENDERED_PARENT = "移除父母性别限制";
		public const string GLOBAL_CONFIG = "[全局配置]";
		public const string SEARCH = "搜索 ";

		public const string AdvancedMode = "AdvancedMode";
		public const string UseRaceSpecific = "UseRaceSpecific";
		public const string CustomAging = "CustomAging";
		public const string UngenderedParent = "UngenderedParent";

		public static string Search_Buffer = "";

		//public static bool AdvancedMode = false;

		public static Vector2 scrollVector = Vector2.zero;
		public static float scrollHeight = 0f;

		public static List<ThingDef> races = null;

		public static string HEADER_RESET(string v) =>
			$"这将恢复'{v}'设置的所有默认值，且无法撤销。确定要这样做吗？";

		public static void Draw_Root_Race_Reset(ThingDef race)
		{
			Find.WindowStack.Add(new Dialog_MessageBox(
				HEADER_RESET(race != null ? race.defName : GLOBAL_CONFIG),
				YES,
				() =>
				{
					new State(race, Gender.Female).Clear();
					new State(race, Gender.Male).Clear();
				},
				NO
			));
		}

		public static void Draw_Root_Race(ThingDef race)
		{
			void Callback(int i)
			{
				switch (i)
				{
					case 0:
						new EditWindow(race);
						break;
					case 1:
						if (race != null)
							new CopyWindow(race);
						else
							Draw_Root_Race_Reset(race);
						break;
					case 2:
						Draw_Root_Race_Reset(race);
						break;
				}
			}

			if (race != null)
				new ComboWindow(
					Callback,
					$"[{race.defName}] {race.LabelCap}",
					race.DescriptionDetailed,
					EDIT,
					COPY_TO,
					RESET
				);
			else
				new ComboWindow(
					Callback,
					GLOBAL_CONFIG,
					DESCRIPTION_GLOBAL_CONFIG,
					EDIT,
					RESET
				);
		}

		public static void Draw_Root(Listing_Standard gui, Rect inRect)
		{
			if (races == null)
			{
				races = new List<ThingDef> { null };
				
				foreach (ThingDef def in DefDatabase<ThingDef>.AllDefs)
					if (def.race != null)
						races.Add(def);
			}

			float width = gui.ColumnWidth;

			gui.ColumnWidth = width * 0.5f;
			{
				Tools.Bool(gui, State.GLOBAL, AdvancedMode, ADVANCED_MODE, DESCRIPTION_ADVANCED_MODE);
				Tools.Bool(gui, State.GLOBAL, UseRaceSpecific, USE_RACE_SPECIFIC, DESCRIPTION_USE_RACE_SPECIFIC);

				bool _CustomAging = Tools.Bool(
					gui,
					State.GLOBAL,
					out bool _CustomAgingUpdated,
					CustomAging,
					CUSTOM_AGING,
					DESCRIPTION_CUSTOM_AGING
				);

				bool _UngenderedParent = Tools.Bool(
					gui,
					State.GLOBAL,
					out bool _UngenderedParentUpdated,
					UngenderedParent,
					UNGENDERED_PARENT,
					DESCRIPTION_UNGENDERED_PARENT
				);

				//gui.CheckboxLabeled(ADVANCED_MODE, ref AdvancedMode, DESCRIPTION_ADVANCED_MODE);


				// Patch/unpatch hooks since this is heavy on performance.

				if (_CustomAgingUpdated)
					if (_CustomAging)
						Patch_Pawn_AgeTracker_AgeTickInterval.module.Patch();
					else
						Patch_Pawn_AgeTracker_AgeTickInterval.module.Unpatch();

				if (_UngenderedParentUpdated)
					if (_UngenderedParent)
					{
						Patch_ParentRelationUtility_GetFather.module.Patch();
						Patch_ParentRelationUtility_GetMother.module.Patch();
					}
					else
					{
						Patch_ParentRelationUtility_GetFather.module.Unpatch();
						Patch_ParentRelationUtility_GetMother.module.Unpatch();
					}
			}

			gui.Gap(20f);


			// Basic Settings

			if (!State.GLOBAL.Bool(AdvancedMode))
			{
				if (gui.ButtonText(SHOW_CONFIG))
					new EditWindow();

				if (gui.ButtonText(RESET))
					Find.WindowStack.Add(new Dialog_MessageBox(
						HEADER_RESET(GLOBAL_CONFIG),
						YES,
						() => new State(null).Clear(),
						NO
					));

				return;
			}


			// Advanced Settings

			if (gui.ButtonText(RESET_ALL))
				Find.WindowStack.Add(new Dialog_MessageBox(
					DESCRIPTION_RESET_ALL,
					YES,
					() =>
					{
						new State(null).Clear();

						foreach (ThingDef race in races)
							new State(race).Clear();
					},
					NO
				));

			Search_Buffer = gui.TextEntryLabeled(SEARCH, Search_Buffer);

			float height = gui.CurHeight;

			Widgets.BeginScrollView(
				new Rect(
					0f,
					height,
					gui.ColumnWidth + 20f,
					inRect.height - height - 40f
				),
				ref scrollVector,
				new Rect(
					0f,
					height,
					gui.ColumnWidth - 16f,
					//inRect.height + height - 40f + races.Count * 24f
					scrollHeight
				)
			);
			{
				foreach (ThingDef race in races)
					if (race != null)
					{
						if (Search_Buffer.Length == 0 ||
							race.defName.ToLower().Contains(Search_Buffer) ||
							race.LabelCap.ToLower().ToStringSafe().Contains(Search_Buffer))
							if (gui.ButtonText(race.defName))
								Draw_Root_Race(race);
					}
					else if (gui.ButtonText(GLOBAL_CONFIG))
						Draw_Root_Race(null);

				scrollHeight = gui.CurHeight - height;
			}
			Widgets.EndScrollView();
		}
	}
}
