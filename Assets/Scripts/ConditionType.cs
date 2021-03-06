/// <summary>
/// コンディションの種類
/// </summary>
public enum ConditionType
{
    Poison,       //毒の場合は体力を減らす。移動後にHPダメージ
    Hide_Symbols, //全てのシンボルが見えないが、透過すれば取れる。エネミーも先頭になる
    View,         //視界の増減2.5〜6.0f
    View_Wide,
    View_Narrow,
    Untouchable,  //アイテムやオープンのシンボルが見えない上に取れない。エネミーも戦闘になる
    Walk_through, //エネミーのシンボルを戦闘なしで透過できる
    Sleep,        //睡眠(移動不可）の場合は足踏みしかできないように入力制限する
    Confusion,    //混乱(停止不可)の場合はランダムな移動しかできないように入力制限する
    Fatigue,      //疲労の場合は攻撃力が半減（これはコンディションでの効果）
    Disease,      //病気の場合は移動速度が半減（これはコンディションへの効果）
    Curse,        //呪い(アイテム取得不可)の場合はエネミーのシンボルのみのエンカウント(これはMapController)
}