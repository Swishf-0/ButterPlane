# ButterPlaneGimmick

[![サンプル動画](http://img.youtube.com/vi/35mnL1nWAWI/0.jpg)](https://www.youtube.com/watch?v=35mnL1nWAWI)  
https://www.youtube.com/watch?v=35mnL1nWAWI

飛行機がワールドを飛び回るギミックです  
カラフルな飛行機雲を出したり、文字を描くことができます。  



# 導入

`ButterPlane.unitypackage` から導入してください。  


## サンプルシーン

`ButterPlane > Scenes > ButterPlane` がサンプルシーンとなっています。  
ギミックを実際に試すことができます。  
Unity上での実行や、ローカルワールドでの実行、VRCへのアップロードも可能です。  
<img height=300 src=res/images/1.png>


## ギミックの設置
`ButterPlane > Prefabs > ButterPlane` がギミックプレハブとなっています。  
これをワールドに設置することでギミックが動作します。  
<img height=300 src=res/images/2.png>



# コースの設定


## コースのプレビュー
コースのプレビューは `Scene` ビューにて `Gizmos` をONにすることで見ることができます。  
<img height=500 src=res/images/3.png>


## コースオブジェクト
コースは複数のアンカーオブジェクトでできています。  
アンカーオブジェクトを移動することでコースの形を変更できます。  
アンカーオブジェクトは任意に複製可能です。  
<img height=400 src=res/images/4.png>  


## コースの切り替え
コースは `Course` スクリプトの `Course Anchor Root` を差し替えることで変更できます。  
また、コースのプレビューは `Course Visualizer` スクリプトの `Show Line` でも切り替えられます。  
(ギミック実行中の切り替えはできません、作成段階のバックアップなどで使用できます)  
<img height=400 src=res/images/5.png>



# エフェクト設定


## 飛行機雲エフェクトの設定
飛行機は `Plane` オブジェクト以下にあります。  
`Plane Effect` スクリプトの `Effects` に飛行機雲のエフェクトが設定されています。  
各エフェクトのプレハブを編集することで飛行機雲の見た目を変えることができます。  
<img height=300 src=res/images/6.png>


## コース上の特定の地点を通過するとエフェクトが出るようにする
`Plane Effect Info` スクリプトとコライダーを持つオブジェクトを設置することで、そのコライダーを通過した際にエフェクトを出すことができます。  
`EffectColliders` オブジェクト以下のオブジェクトを参考に設定してください。  
　`Letter Effect`: `Letters`に設定した文字が描かれます  
　その他: 設定されている各飛行機雲エフェクトが、`Duration` に設定した秒数だけ出ます。 (`__MAX__` は関係ありません)
<img height=300 src=res/images/7.png>  


## ランダムでエフェクトが出るようにする
`UdonScript` オブジェクトの `Plane Effect Controller` で自動でエフェクトが出る設定を行えます。  
　`Random Letter Mode`: `Random Letters` に設定した文字をランダムで描きます  
　`Random Smoke Mode`: 飛行機雲エフェクトをランダムに表示します  
表示時間や表示間隔の設定はその他のパラメータで行えます。  
<img height=500 src=res/images/8.png>  


## 文字エフェクトの設定
飛行機雲で描く文字の見た目の設定を行えます。  
　文字を見る方向: 飛行機雲を見る方向を設定します。その方向から見たときに読める向きで描かれます。  
　文字の間隔: 文字どうしの間隔  
　文字の幅: 文字の横幅です。数値によって引き伸ばされたり縮んだりします。飛行機の速さなどに合わせて設定してください。  
　スペースの幅: スペース (空文字) の幅  
<img height=300 src=res/images/9.png>  


## 文字エフェクトの表示時間の設定
`smoke_letter` プレハブの `StartLifetime` を変更します。  
デフォルトでは30秒～35秒表示されるようになっています。  
<img height=300 src=res/images/18.png>  



# 飛行機設定


## 飛行機の飛び方設定
`UdonScript` オブジェクトの `Plane Controller` で飛行機の飛び方の設定を行えます。  
　飛行機どうしの間隔: 飛行機どうしの間隔です。文字エフェクトを使用する場合は文字の形にも影響します。  
　飛行機の速さ: 飛行機の速さです。文字エフェクトを使用する場合は文字の形にも影響します。  
<img height=300 src=res/images/10.png>  


## 飛行機の揺れ設定
飛行機は飛行中にカーブに合わせて傾いたりランダムに揺れるように設定されています。  
設定は `Plane` オブジェクトの `Plane` スクリプトから行えます。  
不要な場合は `Use Roll Action` のチェックを外すかスクリプトをオブジェクトから取り外します。  
<img height=300 src=res/images/11.png>  


## 飛行機の見た目設定、モデル調整
飛行機の見た目やモデルは `mesh` プレハブ、`Plane` プレハブから行います。  
<img height=500 src=res/images/12.png>  



# 文字エフェクトの文字の形編集
`ButterPlane > Scenes > LetterDataSetEditor` シーンから文字の編集を行えます。  
<img height=300 src=res/images/13.png>  

このシーンを開くと文字データが並んでいます。  
<img height=400 src=res/images/14.png>  

`LetterDataSet` オブジェクトの下に各文字のデータが入っています。  
`Letter Data Edit Tool` は編集に便利なツールになっています。  
<img height=500 src=res/images/15.png>  

<img height=300 src=res/images/16.png>  

<br>

`LetterDataSet` オブジェクトの `Letter Data Set Generator` スクリプトの「データ変換」ボタンをクリックするとデータが反映されます。  
通常であればそのまま `ButterPlane` シーンを再度開けばデータが反映されている状態になります。  
(`Letter Data Set Prefab` に設定されているプレハブに反映されます)  
<img height=200 src=res/images/17.png>  
