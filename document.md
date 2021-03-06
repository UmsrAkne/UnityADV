# scenario.xml の仕様

## scenario

シナリオタグで挟み込んだものを一つの単位として読み込む。

    <scenario>
        <text str="message">
    </scenario>

    <scn>
        <text str="message">
    </scn>

`scenario, scn` 何れも同じ意味。どちらも可。  
以降に記述する全てのタグは `scenario, scn` の子として記述する。外に書いてもエラーにはならないが認識はされない。

## text

- 属性
    - string : テキストを表示します
    - str : テキストを表示します


## voice

ボイスを再生します。

- 属性
    - int number : フォルダ内のファイルの通し番号
    - string fileName : ファイル名を指定
    - int channel : チャンネル番号を指定します。デフォルトは `0` 。指定可能なのは `0-2` です。


記述例。

    <voice number="1" />
    <voice fileName="sound" />

## image

画像を表示する。このタグ指定するとデフォルトで `AlphaChanger` アニメーションが自動で追加される。

- 属性
    - string a : 画像のファイル名を指定します 
    - string b : 画像のファイル名を指定します 
    - string c : 画像のファイル名を指定します 
    - string d : 画像のファイル名を指定します 
    - int x : 画像の位置を指定します
    - int y : 画像の位置を指定します
    - double scale : 画像の拡大率を指定します
    - int angle : 画像の角度を指定します
    - string mask : マスク画像を指定します

画像は奥側のレイヤーから `a -> b -> c -> d` の順番で重なる。`a` が一番奥。`d` が一番手前のレイヤーとなる。

記述例

    <image a="" b="" c="" d="" x="0" y="0" scale="1.0" mask="" />

## draw

既に生成されている画像に対して、上書きする形で画像を追加します。

- 属性
    - string a : 画像のファイル名を指定します 
    - string b : 画像のファイル名を指定します 
    - string c : 画像のファイル名を指定します 
    - string d : 画像のファイル名を指定します 
    - double depth : 一回の処理での上書きの濃さを指定します

記述例

    <draw a="" b="" c="" d="" depth="0.1" />

## se

効果音を鳴らす。

- 属性
    - int number : commonResouce/ses フォルダ内の通し番号で指定します
    - string fileName : commonResouce/ses フォルダ内のファイル名で指定します
    - int repeatCount : 繰り返し回数を指定します。（現在は一回か無限回のみ対応）

記述例

    <!-- 繰り返し回数2回以上は無限ループになる仕様 -->
    <se fileName="sound" repeatCount="2" />

## backgroundVoice

bgv を鳴らす

- 属性
    - string names : ファイル名を `,` 区切りで入力します。`,`周辺に半角スペースを入れることが可能です
    - int channel : チャンネル番号を指定します。デフォルトは `0` 。指定可能なのは `0-2` です

記述例

    <backgroundVoice names="v1, v2, v3, v4" channel="1" />

## stop

指定した対象の動きを止めます。

- 属性
    - string target
    - int layerIndex
    - int channel
    - int name

## anime

- 属性
    - string name : アニメーションの名前を入力します。入力可能なアニメーションは以下

### alphaChanger 
### shake
### slide 
### flash 
### bound