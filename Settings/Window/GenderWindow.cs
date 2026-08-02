using UnityEngine;
using Verse;

namespace RW_CustomPawnGeneration
{
	public class GenderWindow : BaseWindow
	{
		public const string DESCRIPTION_SEPARATE_GENDER =
			"启用后，将为男性和女性分开统计数据。";
		public const string DESCRIPTION_OVERRIDE_GENDER =
			"启用后，允许你设置哪种性别出现频率更高。";
		public const string DESCRIPTION_UNFORCED_GENDER =
			"部分角色在生成时具有\"强制\"性别" +
			"（PawnGenerationRequest.FixedGender）" +
			"（与背景故事相关或作为另一个角色的父亲/母亲生成）。" +
			"启用此选项将忽略它。" +
			"可能导致一些小问题（单亲父亲/母亲），" +
			"但不会影响游戏运行。";
		public const string DESCRIPTION_MODIFY_AGGRESSIVELY =
			"启用后，" +
			"由性别变更引起的一些错误将被忽略。" +
			"为保证模组兼容性，建议启用此选项。";

		public const string SEPARATE_GENDER = "分开性别统计";
		public const string OVERRIDE_GENDER = "覆盖性别频率";
		public const string UNFORCED_GENDER = "覆盖强制性别";
		public const string MODIFY_AGGRESSIVELY = "激进修改模式";

		public const string MALE = "男性";
		public const string FEMALE = "女性";

		public const string SeparateGender = "SeparateGender";
		public const string OverrideGender = "OverrideGender";
		public const string UnforcedGender = "UnforcedGender";
		public const string ModifyAggressively = "ModifyAggressively";
		public const string GenderSlider = "GenderSlider";

		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(480f, 320f);
			}
		}

		public GenderWindow(ThingDef race) : base(race)
		{
		}

		public override void Draw_Inside(Rect inRect, Listing_Standard gui)
		{
			int _GenderSlider = state.Get(GenderSlider);
			{
				Tools.GBool(gui, state, SeparateGender, SEPARATE_GENDER, DESCRIPTION_SEPARATE_GENDER);
				Tools.GBool(gui, state, UnforcedGender, UNFORCED_GENDER, DESCRIPTION_UNFORCED_GENDER);
				Tools.GBool(gui, state, OverrideGender, OVERRIDE_GENDER, DESCRIPTION_OVERRIDE_GENDER);
				Tools.GBool(gui, state, ModifyAggressively, MODIFY_AGGRESSIVELY, DESCRIPTION_MODIFY_AGGRESSIVELY);

				if (state.GBool(OverrideGender))
				{
					gui.Gap(10f);

					gui.LabelDouble($"{100 - _GenderSlider}% {MALE}", $"{_GenderSlider}% {FEMALE}");
					_GenderSlider = (int)gui.Slider(_GenderSlider, 0, 100);
				}
			}
			state.Set(GenderSlider, _GenderSlider);
		}
	}
}
