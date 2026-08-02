using RimWorld;
using UnityEngine;
using Verse;

namespace RW_CustomPawnGeneration
{
	public class BodyWindow : BaseWindow
	{
		public const string DESCRIPTION_BODY_FIX =
			"部分背景故事会赋予角色异性平均体型。" +
			"启用此选项将禁用它。";
		public const string DESCRIPTION_FILTER_BODY =
			"启用后，允许你禁用体型。" +
			"未勾选的体型将被禁用。" +
			"至少需要保留1种体型。" +
			"仅适用于人类。";

		public const string FILTER_BODY = "过滤体型";
		public const string FilterBody = "FilterBody";

		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(560f, 320f);
			}
		}

		public BodyWindow(ThingDef race, Gender? gender = null) : base(race, gender)
		{
		}

		public override void Draw_Inside(Rect inRect, Listing_Standard gui)
		{
			Tools.GBool(gui, state, FilterBody, FILTER_BODY, DESCRIPTION_FILTER_BODY);

			if (state.GBool(FilterBody))
				foreach (BodyTypeDef def in DefDatabase<BodyTypeDef>.AllDefs)
				{
					if (def == BodyTypeDefOf.Baby)
						continue;

					if (def == BodyTypeDefOf.Child)
						continue;

					Tools.Bool(gui, state, $"{FilterBody}|{def.defName}", def.defName);
				}
		}
	}
}
