# NextaCordingTest

# 概要
- 参加者のリストと、どの参加者が何点をいつ取得したのかのリストを入力する。
- そうすると、得点のトップ10が出力される。
- 同点の場合、最終更新日が古い方が上位とする。

# 操作方法
- 参加者のリストは、,区切りでプレイヤー名を入力する。
- どの参加者が何点をいつ取得したのかのリストは、「参加者,点数,年月日８桁」の形で１行ずつ入力する。（例：tarou,100,20231122）
  - 参加者にいない名前の場合や年月日が８桁でなかった場合は入力がキャンセルされ、もう一度入力待ちされる
- ランキングが表示された後、何かキーを押すと終了する。

# 考慮事項
- rankerCountを変更するとtop何人を表示するか変更できる
- 同率を表示する場合も考慮したが、トップ１０人を表示するように変更した
