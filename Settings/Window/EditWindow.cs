using UnityEngine;
using Verse;

namespace RW_CustomPawnGeneration
{
	public partial class EditWindow : BaseWindow
	{
		public static string[] COMBO_BOOL = new string[]
		{
				"已禁用",
				"已启用"
		};

		public static string[] COMBO_GLOBAL_BOOL = new string[]
		{
				"使用全局配置",
				"已禁用",
				"已启用"
		};

		public const string GENDER = "性别";
		public const string AGE = "年龄";
		public const string BODY = "体型";
		public const string TRAITS = "特质";
		public const string HEDIFFS = "健康状况";

		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(560f, 560f);
			}
		}

		public EditWindow(ThingDef race = null) : base(race)
		{
		}

		public override void Draw_Inside(Rect inRect, Listing_Standard gui)
		{
			if (hasGenders)
			{
				if (gui.ButtonText(GENDER))
					new GenderWindow(race);

				gui.Gap(20f);
			}

			float height = gui.CurHeight;
			float width = gui.ColumnWidth;

			bool gender;

			if (race != null)
				gender = Settings.Bool(new Settings.State(null), state, GenderWindow.SeparateGender);
			else
				gender = state.Bool(GenderWindow.SeparateGender);

			gui.ColumnWidth = width / 2f - 8f;
			{
				Draw_Gender(inRect, gui, gender ? (Gender?)Gender.Male : null);
			}

			if (!gender)
				return;

			gui.NewColumn();
			gui.Gap(height);
			{
				Draw_Gender(inRect, gui, Gender.Female);
			}
		}

		public void Draw_Gender(Rect inRect, Listing_Standard gui, Gender? gender = null)
		{
			if (gender != null)
			{
				gui.Label(gender.Value.ToString());
				gui.Gap(10f);
			}

			if (gui.ButtonText(AGE))
				new AgeWindow(race, gender);

			if (isHumanlike)
			{
				if (gui.ButtonText(BODY))
					new BodyWindow(race, gender);

				if (gui.ButtonText(TRAITS))
					new TraitsWindow(race, gender);
			}

			if (race != null &&
				gui.ButtonText(HEDIFFS))
				new HediffWindow(race, gender);
		}
	}
}
