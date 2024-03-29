ver 2023.06.11

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
以降に記述する全てのタグは `scenario, scn` の子として記述する。

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
    - bool InheritStatus :   
      true に指定すると、画像の描画時に最新の画像の状態 (位置、拡大率) をコピーします。

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
    - int number : commonResource/ses フォルダ内の通し番号で指定します
    - string fileName : commonResource/ses フォルダ内のファイル名で指定します
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
    - string name

主要なアニメーションの停止機能を実装済。以下のように記述して使用する。

記述例
  
    <stop target="anime" name="shake" />
    <stop target="anime" name="animationChain" />

-------------------------------------------------------

## start

この要素を子に持つシナリオがある場合、そのシナリオよりも前のシナリオは読み込み時にパースされずにスキップされます。

つまり、この要素を持つシナリオからシーンが開始します。

複数の `start` が設定されている場合、先に登場した `start` が優先されます。

記述例

    <scenario> <start /> <text str="test" /> </scenario>

## ignore

この要素を子に持つシナリオは読み込み時、パースされずにスキップされます。

## anime

`name` 属性に記述するアニメーションは後述を参照。  
続いて、有効な属性を追記するという形でアニメーションを宣言する。

アニメーションによって、有効な属性が異なる。  
無効(未定義)な属性を指定しても、XML として解釈可能であれば問題なく動作する。

- 属性
    - string name : アニメーションの名前を入力します。入力可能なアニメーションを以下を参照

### alphaChanger 
### shake

- 属性
    - int StrengthX 
    - int StrengthY 
    - int Duration  = 60;
    - int RepeatCount 
    - int Interval 
    - int Delay
    - string GroupName

### slide 

画面をスライドさせます。`Duration` に `0` 以外の値を指定すると、内部で動作ロジックが変化します。

`Duration` に値を指定した場合、`Speed` は自動設定され、指定した値分丁度の時間でアニメーションを行います。

- 属性
    - double Speed = 1.0;
    - int Degree
    - int Distance
    - int Duration
    - int RepeatCount
    - int Interval
    - string GroupName

### flash 

画面を白発光させます。Duration に指定したフレーム数の間に１回発光します。

- 属性
    - int Duration  = 40;
    - int RepeatCount  = 1; 
    - int Delay  = 0;
    - int Interval = 0;
    - string GroupName

### bound

### animationChain

- 属性
    - int RepeatCount = 0;
  
特殊なアニメーションタグです。内部に anime 要素を入力して使用します。

```
  <anime name="animationChain">
    <!-- shake, slide の順でアニメーションが実行される -->
    <anime name="shake" />
    <anime name="slide" />
  </anime>
```

この要素の子のアニメーションは、記述された順番で順次実行され、この要素自体が単体のアニメーションとして扱われます。

ただし、同じ `groupName` 属性のアニメーションが複数ある場合は、属性付きのアニメーションは同時実行されます。

```
  <anime name="animationChain">
    <!-- shake, slide に同じグループ名を指定すると同時に動作する -->
    <anime name="shake" groupName="sampleGroup" />
    <anime name="slide" groupName="sampleGroup" />
  </anime>
```

`groupName` 属性で同時にアニメーションを動かした場合、両方のアニメーションが停止するまで次のアニメーションは再生されません。

Image 要素が入力された場合、他のアニメーション同様、このアニメーションはストップします。

### chain

`animationChain` の別名です。nameにこれを指定した場合は、`animationChain` が生成されます。

### draw

- 属性
  - string A;
  - string B;
  - string C;
  - int X = 0;
  - int Y = 0;
  - double Scale = 1.0;
  - int Wait = 0;

### scaleChange

画像を拡大、縮小します。現在の拡大率から `To` で指定した倍率まで、`Duration` に指定した時間をかけて変化します。

  - double To 
  - int Duration 
  - int RepeatCount // 未実装
  - int Delay 
  - string GroupName

記述例

以下のように記述します。

アニメーション開始時点での拡大率が 1.0 であった場合、
100フレームをかけて 1.0 -> 1.5 まで拡大率が変化します。

    <anime name="scaleChange" to="1.5" duration="100" />

# setting.xml の仕様

各シーンの `texts` ディレクトリ以下に `setting.xml` と命名して配置する。  
以下のようにルートを `<setting>`  として使用する。

    <setting>
      <defaultSize width="1280" height="720" />
      <bgm number="1" />
    </setting>

実装済みのタグを以下に示す。

## bgm

シーンで流れる BGM を番号、ファイル名で指定する。  
BGM は `commonResource/bgms` の `.ogg` ファイルのみがカウントの対象となる。  
デフォルトは `0` となっている。

- 属性
  - int number : BGM の番号を指定します。インデックスは 0 始まり。デフォルトは 0 
  - string fileName : 再生する BGM をファイル名で指定します。 `number` と一緒に指定した場合は、`fileName` が優先されます。
  - float volume : BGM の音量を指定します。`0 - 1.0` の範囲で設定します。  
  デフォルトは 1.0 です。

## defaultSize

未実装