# memo

## dotnetコマンド

- dotnet new
  - プロジェクトの作成
- dotnet tool
  - ライブラリなどの管理
  - 例: `dotnet tool install dotnet-ef`


##  asp.net全体、特にDIコンテナ

### ルーティング

- 参考:[microsoft](https://learn.microsoft.com/ja-jp/aspnet/core/fundamentals/routing?view=aspnetcore-7.0)

### パイプライン

- 参考: [microsoft](https://learn.microsoft.com/ja-jp/aspnet/core/fundamentals/middleware/?view=aspnetcore-7.0)

### Filter

- 参考: [microsoft](https://learn.microsoft.com/en-us/aspnet/core/mvc/controllers/filters?view=aspnetcore-7.0)
- リクエスト処理パイプラインの特定の段階の前または後にコードを実行することができます。
- 以下のフィルターがある
  - Authorization filters
  - Resource filters
  - Action filters
  - Endpoint filters
  - Exception filters
  - Result filters
- 以下の順序で実行される

![](images/2023-04-17-13-18-16.png)

#### ActionFilter

- null

## 認証認可のやり方

- JWT。
  - 参考: https://jasonwatmore.com/react-18-authentication-with-net-6-aspnet-core-jwt-api
  - [翻訳(途中まで)](dotnet6.0_JWT_Authentication_Tutorial_with_Example_API.md)

## ロギングのやり方

- null

## コントローラーのいい感じの書き方

## REST APIの設計

- RESTfulにしたい。
  - https://jasonwatmore.com/post/2021/05/25/net-5-simple-api-for-authentication-registration-and-user-management
  - [翻訳(途中まで)](simple_API_for_authentication_registration_and_user_management.md)

## github actions

- null

## Azure構成

- デプロイ方法参考: https://hub.uni-face.co.jp/nextjs-csharpapi-deploy-to-azure/

## 複数プロジェクト

- https://qiita.com/unyorita/items/8a92cb19b618e8e4a4a5

## C#11.0の新機能

- [C# 11.0](https://learn.microsoft.com/ja-jp/dotnet/csharp/whats-new/csharp-11)
- [C# 10.0](https://learn.microsoft.com/ja-jp/dotnet/csharp/whats-new/csharp-10)
- [C# 9.0](https://learn.microsoft.com/ja-jp/dotnet/csharp/whats-new/csharp-9)
- [C# 8.0](https://learn.microsoft.com/ja-jp/dotnet/csharp/whats-new/csharp-8)

## docker

- null