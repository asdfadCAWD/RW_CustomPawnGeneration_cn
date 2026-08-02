using UnityEngine;
using Verse;

namespace RW_CustomPawnGeneration
{
	public class AgeWindow : BaseWindow
	{
		public const string DESCRIPTION_AGE_CURVE =
			"启用后，将尝试将角色当前年龄" +
			"转换到新的年龄限制。这可能不够精确，但会" +
			"产生与原版年龄变化类似的效果。" +
			"禁用此选项则只限制角色的年龄。";
		public const string DESCRIPTION_HAS_MIN_AGE =
			"启用后，将确保所有角色" +
			"无法低于某个年龄。" +
			"这也会影响怀孕模组，" +
			"意味着新生婴儿将从最低年龄开始。";
		public const string DESCRIPTION_MIN_AGE_SOFT =
			"启用后，低于最低年龄生成的" +
			"角色将不会被更改（例如婴儿角色）。";
		public const string DESCRIPTION_HAS_MAX_AGE =
			"启用后，将确保所有角色" +
			"无法高于某个年龄。";
		public const string DESCRIPTION_MAX_AGE_CHRONO =
			"启用后，如果角色的生物年龄超过了最高年龄，" +
			"每次生日时将增加其历法年龄。";
		public const string DESCRIPTION_HAS_AGE_TICK =
			"启用后，允许你更改角色的年龄增长速度。";

		public const string AGE_CURVE = "保持年龄曲线";
		public const string HAS_MIN_AGE = "启用最低年龄";
		public const string MIN_AGE_SOFT = "不影响低于最低年龄的角色";
		public const string MIN_AGE = "最低年龄 ";
		public const string HAS_MAX_AGE = "启用最高年龄";
		public const string MAX_AGE_CHRONO = "将超出的生物年龄添加为历法年龄";
		public const string MAX_AGE = "最高年龄 ";
		public const string HAS_AGE_TICK = "覆盖年龄刻";
		public const string AGE_TICK = "年龄刻速度 [默认: 1] ";

		public const string AgeCurve = "AgeCurve";
		public const string HasMinAge = "HasMinAge";
		public const string MinAgeSoft = "MinAgeSoft";
		public const string MinAge = "MinAge";
		public const string HasMaxAge = "HasMaxAge";
		public const string MaxAgeChrono = "MaxAgeChrono";
		public const string MaxAge = "MaxAge";
		public const string HasAgeTick = "HasAgeTick";
		public const string AgeTick = "AgeTick";

		public string _MinAgeBuffer = "";
		public string _MaxAgeBuffer = "";
		public string _AgeTickBuffer = "";

		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(480f, 480f);
			}
		}

		public AgeWindow(ThingDef race, Gender? gender = null) : base(race, gender)
		{
			_MinAgeBuffer = state.Get(MinAge).ToString();
			_MaxAgeBuffer = state.Get(MaxAge).ToString();
			_AgeTickBuffer = state.Get(AgeTick).ToString();
		}

		public override void Draw_Inside(Rect inRect, Listing_Standard gui)
		{
			bool _HasMinAge = state.GBool(HasMinAge);
			bool _HasMaxAge = state.GBool(HasMaxAge);
			bool _HasAgeTick = state.GBool(HasAgeTick);
			int _MinAge = state.Get(MinAge);
			int _MaxAge = state.Get(MaxAge);
			int _AgeTick = state.Get(AgeTick);

			Tools.GBool(gui, state, AgeCurve, AGE_CURVE, DESCRIPTION_AGE_CURVE);
			Tools.GBool(gui, state, MaxAgeChrono, MAX_AGE_CHRONO, DESCRIPTION_MAX_AGE_CHRONO);
			Tools.GBool(gui, state, HasMinAge, HAS_MIN_AGE, DESCRIPTION_HAS_MIN_AGE);

			if (_HasMinAge)
			{
				Tools.GBool(gui, state, MinAgeSoft, MIN_AGE_SOFT, DESCRIPTION_MIN_AGE_SOFT);

				gui.TextFieldNumericLabeled(
					MIN_AGE,
					ref _MinAge,
					ref _MinAgeBuffer,
					0,
					_HasMaxAge ? _MaxAge : 1E+09f
				);
				state.Set(MinAge, _MinAge);

				gui.Gap(10f);
			}

			Tools.GBool(gui, state, HasMaxAge, HAS_MAX_AGE, DESCRIPTION_HAS_MAX_AGE);

			if (_HasMaxAge)
			{
				gui.TextFieldNumericLabeled(
					MAX_AGE,
					ref _MaxAge,
					ref _MaxAgeBuffer,
					_HasMinAge ? _MinAge : 0
				);
				state.Set(MaxAge, _MaxAge);
			}

			Tools.GBool(gui, state, HasAgeTick, HAS_AGE_TICK, DESCRIPTION_HAS_AGE_TICK);

			if (_HasAgeTick)
			{
				gui.TextFieldNumericLabeled(
					AGE_TICK,
					ref _AgeTick,
					ref _AgeTickBuffer
				);

				state.Set(AgeTick, _AgeTick);
			}
		}
	}
}
