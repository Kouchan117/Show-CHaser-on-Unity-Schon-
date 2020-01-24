Show-CHaser-on-Unity(Schon)
====

overview
## chsr
* Unityプロジェクトファイル
## CHaser-Server-master
* 使用させていただいたCHaserサーバー
* https://github.com/t-akisato/CHaser-Server
## CHaser-master
* 使用させていただいたCHaserクライアント
* https://github.com/u16-kushiro-procon/CHaser

## Description
Rubyで作成されたCHaserサーバーを対象に, Unity上でゲーム状況を再現するアプリケーションです. 
Rubyサーバーから渡されたログをUnity上で読むことで再現を行っています. 
同梱しているCHaser-Server-masterには, 使用させていただいたCHaserサーバーにプレイヤーの点数, マップの情報等のログを出すように手を加えてあります. 

## Demo
![play-schon](https://user-images.githubusercontent.com/15669383/72806547-c1a91280-3c98-11ea-9192-5863e911f2c8.gif)

## Requirement
CHaserサーバー及び, クライアントを実行するためにあらかじめWSLにruby環境を導入する必要があります. 

## Usage

** CHaser-Server-master **
* ruby chsr.rb [-h] : 簡単なヘルプの表示
* ruby chsr.rb [-m] マップ名 : マップの選択(未入力の場合はmap01.mapが選択される)
* ruby chsr.rb [-w] 数字 : 画面表示のウェイトの指定
* ruby chsr.rb [-p] [数字] : ポーズモードで起動します. 
* ruby chsr.rb [-z] : 全角モードで起動

** CHaser-master **
* ruby ファイル名でクライアント起動
* example : ruby Test1.rb
* 起動前にクライアントプログラム内の接続先IPをサーバーと同じに変更を行ってください.

## Install
UnityHubでchsrフォルダを指定してください. 

## Licence
© Unity Technologies Japan/UCL
* [UniRx](https://github.com/neuecc/UniRx/blob/master/LICENSE)
* [CHaser-Server](https://github.com/t-akisato/CHaser-Server/blob/master/LICENSE)

## Update
2020/1/24
* キャラモデルの実装
* サーバーモード及びログリードモードの実装
* プレイヤー移動, ブロックテクスチャ等細かな調整
