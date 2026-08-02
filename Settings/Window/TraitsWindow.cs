using RimWorld;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RW_CustomPawnGeneration
{
	public class TraitsWindow : BaseWindow
	{
		public static string[] COMBO_TRAITS = new string[]
		{
			"正常",
			"已禁用",
			"强制"
		};

		public const string DESCRIPTION_TRAITS_BLOCKED =
			"* 如果角色骰到了已禁用/强制的特质，将重新掷骰。\n" +
			"* 游戏可能会在控制台中输出大量 '[角色] 已拥有 [特质]' 的消息。\n" +
			"* 强制的特质会在生成特质之后添加，" +
			"可能超过最大特质数量。\n" +
			"* 你可以强制相同特质的不同等级。\n" +
			"警告：禁用/强制大多数特质会导致游戏陷入永久循环，" +
			"使你无法正常游玩！请尽量只禁用/强制少于一半的特质。";
		public const string DESCRIPTION_OVERRIDE_TRAITS =
			"允许阻止特质出现在角色身上，" +
			"以及强制所有角色分配某些特质。" +
			"仅适用于人类。";
		public const string DESCRIPTION_RESET =
			"要恢复所有默认值吗？";

		public const string OVERRIDE_TRAITS = "允许强制/禁用特质";

		public const string OverrideTraits = "OverrideTraits";
		public const string Trait = "Trait";

		public string Search = "";

		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(640f, 640f);
			}
		}

		public TraitsWindow(ThingDef race, Gender? gender = null) : base(race, gender)
		{
		}

		public override void Draw_Outside(Rect inRect, Listing_Standard gui)
		{
			Text.Font = GameFont.Tiny;
			{
				gui.Label(DESCRIPTION_TRAITS_BLOCKED);
			}
			Text.Font = GameFont.Small;

			gui.Gap(10f);

			Tools.GBool(gui, state, OverrideTraits, OVERRIDE_TRAITS, DESCRIPTION_OVERRIDE_TRAITS);

			gui.Gap(10f);

			if (!state.GBool(OverrideTraits))
				return;

			Search = gui.TextEntryLabeled(SEARCH, Search).ToLower();

			gui.Gap(10f);

			if (gui.ButtonText(Settings.RESET))
				Find.WindowStack.Add(new Dialog_MessageBox(
					DESCRIPTION_RESET,
					Settings.YES,
					() =>
					{
						foreach (TraitDef def in DefDatabase<TraitDef>.AllDefs)
							try
							{
								foreach (TraitDegreeData data in def.degreeDatas)
									state.Remove($"{Trait}|{def.defName}|{data.degree}");
							}
							catch { }
					},
					Settings.NO
				));
		}

		public override void Draw_Inside(Rect inRect, Listing_Standard gui)
		{
			if (!state.GBool(OverrideTraits))
				return;

			IEnumerable<TraitDef> defs = DefDatabase<TraitDef>.AllDefs;

			foreach (TraitDef def in defs)
				try
				{
					foreach (TraitDegreeData data in def.degreeDatas)
					{
						string label = $"[{def.defName}] {data.label ?? def.label}";

						if (label.ToLower().Contains(Search))
							ComboWindow.Entry(
								gui,
								state,
								$"{Trait}|{def.defName}|{data.degree}",
								label,
								data.description ?? def.description,
								COMBO_TRAITS
							);
					}
				}
				catch { }
		}
	}
}
