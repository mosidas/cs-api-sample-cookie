# simple_API_for_authentication_registration_and_user_management

## 1. 参考

https://jasonwatmore.com/post/2021/05/25/net-5-simple-api-for-authentication-registration-and-user-management

## 2. 和訳＆メモ

### 2.1. ソースコード

- https://github.com/cornflourblue/dotnet-5-registration-login-api

### 2.2. 動作環境

- [.NET SDK](https://dotnet.microsoft.com/download) : .netのランタイムとコマンドラインツール
- [Visual Studio Code](https://code.visualstudio.com/) : エディター&実行
- [C# extension for Visual Studio Code](https://marketplace.visualstudio.com/items?itemName=ms-vscode.csharp) : vs codeの.net環境プラグイン
- postman : API実行用

### 2.3. 起動

- ソースコードをクローンする。
- vscodeで開く。
- コマンド実行「dotnet run」
- 以下のようなログがでる。
```log:
info: Microsoft.Hosting.Lifetime[0]
      Now listening on: http://localhost:4000
info: Microsoft.Hosting.Lifetime[0]
      Application started. Press Ctrl+C to shut down.
info: Microsoft.Hosting.Lifetime[0]
      Hosting environment: Development
info: Microsoft.Hosting.Lifetime[0]
      Content root path: {clone_path}\dotnet-5-registration-login-api
```
- 終わるときはCtrl+C。

### 2.4. API実行

#### 2.4.1. ユーザー登録

- postmanでrequest
  - method:POST
  - URL:http://localhost:4000/users/register
  - header:
    - Content-Type:application/json
  - body:
    - raw
```json
{
    "firstName": "Jason",
    "lastName": "Watmore",
    "username": "jason",
    "password": "my-super-secret-password"
}
```
- send
- response : 200 OK
```json
{
    "message": "Registration successful"
}
- 登録済みだと : 400 Bad Request
```json:
{
    "message": "Username 'jason' is already taken"
}
```

#### 2.4.2. ユーザー認証

- postmanでrequest
  - method:POST
  - URL:http://localhost:4000/users/authenticate
  - header:
    - Content-Type:application/json
  - body:
    - raw
```json
{
    "username": "jason",
    "password": "my-super-secret-password"
}
```
- send
- response : 200 OK
```json
{
    "id": 1,
    "firstName": "Jason",
    "lastName": "Watmore",
    "username": "jason",
    "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpZCI6IjEiLCJuYmYiOjE2NjczNTEwOTQsImV4cCI6MTY2Nzk1NTg5NCwiaWF0IjoxNjY3MzUxMDk0fQ.N5OI6rlRP-hReC9mH8V7I9CUvtZPYniegrlvaZ71j4s"
}
```
#### 2.4.3. 認証済ユーザー取得

- postmanでrequest
  - method:GET
  - URL:http://localhost:4000/users
  - authorization:
    - type:bearer token
    - value:{JWT token}
  - header:
    - なし
  - body:
    - なし
- send
- response : 200 OK
```json
[
    {
        "id": 1,
        "firstName": "Jason",
        "lastName": "Watmore",
        "username": "jason"
    }
]
```

#### 2.4.4. ユーザー情報更新

- postmanでrequest
  - method:PUT
  - URL:http://localhost:4000/users/1 (user_id=1のユーザーの場合)
  - authorization:
    - type:bearer token
    - value:{JWT token}
  - header:
    - Content-Type:application/json
  - body:
    - raw
```json
{
    "firstName": "Foo",
    "lastName": "Bar"
}
```

### 2.5. プロジェクト構成

```log
├─.vscode
├─Authorization
├─Controllers
├─Entities
├─Helpers
├─Migrations
│  ├─SqliteMigrations
│  └─SqlServerMigrations
├─Models
│  └─Users
├─Properties
└─Services
```

#### 2.5.1. Authorization

api のカスタム JWT 認証と認可の実装を担当するクラスが含まれています。

#### 2.5.2. Controllers

ウェブAPIのエンドポイント/ルートを定義します。コントローラは、クライアントアプリケーションからhttpリクエストを経由してウェブAPIに入るためのエントリポイントです。

#### 2.5.3. Migrations

API 用のデータベースを自動的に作成・更新するために使用される /Entities フォルダ内のクラスに基づくデータベース移行ファイルです。マイグレーションはEntity Framework Core CLIで生成されます。この例のマイグレーションは、異なるデータベースプロバイダに対して以下のコマンドで生成されました。

- SQLite EF Core Migrations:

```pwsh
dotnet ef migrations add InitialCreate --context SqliteDataContext --output-dir Migrations/SqliteMigrations
```
- SQL Server EF Core Migrations (Windows Command):

```pwsh
set ASPNETCORE_ENVIRONMENT=Production
dotnet ef migrations add InitialCreate --context DataContext --output-dir Migrations/SqlServerMigrations
```
- SQL Server EF Core Migrations (Windows PowerShell):

```pwsh
$env:ASPNETCORE_ENVIRONMENT="Production"
dotnet ef migrations add InitialCreate --context DataContext --output-dir Migrations/SqlServerMigrations
```

- SQL Server EF Core Migrations (MacOS):

```pwsh
ASPNETCORE_ENVIRONMENT=Production dotnet ef migrations add InitialCreate --context DataContext --output-dir Migrations/SqlServerMigrations
```

#### 2.5.4. Models

コントローラメソッドのリクエストモデルとレスポンスモデルを表します。
リクエストモデルは受信するリクエストのパラメータを定義し、レスポンスモデルは返されるデータを定義します。

#### 2.5.5. Services

ビジネスロジック、検証、データベースアクセスのコードが含まれる。

#### 2.5.6. Entities

データベースに格納されているアプリケーションデータを表現します。
Entity Framework Core (EF Core) は、データベースからのリレーショナルデータを、データ管理やCRUD操作のためにアプリケーション内で使用されるC#エンティティオブジェクトのインスタンスにマッピングします。

#### 2.5.7. Helpers

- 上記のフォルダーに入らないもの。


### 2.6. コード

#### 2.6.1. /Authorization/AllowAnonymousAttribute.cs

カスタムの [AllowAnonymous] 属性は、 [Authorize] 属性でデコレートされたコントローラの指定したアクションメソッドに 匿名でアクセスできるようにするために使用されます。
これは、users コントローラで register および login アクションメソッドへの匿名アクセスを許可するために使用します。
下記のカスタム authorize 属性は、アクションメソッドが [AllowAnonymous] で装飾されている場合、認証をスキップします。

私は、一貫性と名前空間間の曖昧な参照エラーを避けるために、カスタムの匿名許可属性 (組み込みのものを使用するのではなく) を作成しました。

#### /Authorization/AuthorizeAttribute.cs

カスタムの [Authorize] 属性は、コントローラや指定したアクションメソッドへのアクセスを制限するために使用します。
認可されたリクエストのみが、[Authorize] 属性が設定されたアクションメソッドへの アクセスを許可されます。
コントローラに [Authorize] 属性が設定されている場合は、 すべてのアクションメソッドが認証済みリクエストに制限されます。
ただし、上記の [AllowAnonymous] 属性は例外です。
認可は OnAuthorization メソッドで行われ、現在のリクエストに認証されたユーザが存在するかどうかをチェックします (context.HttpContext.Items["User"])。
リクエストに有効なJWTアクセストークンが含まれている場合、カスタムjwtミドルウェアによって認証されたユーザーがアタッチされます。
認証に成功した場合、アクションは実行されず、リクエストはコントローラのアクションメソッドに渡され、認証に失敗した場合は、401 Unauthorizedレスポンスが返されます。


#### /Authorization/JwtMiddleware.cs

カスタム JWT ミドルウェアは、リクエストの Authorization ヘッダー (ある場合) から JWT トークンを抽出し、jwtUtils.ValidateToken() メソッドを使用してそれを検証します。
検証が成功した場合、トークンからユーザー ID が返され、認証されたユーザー オブジェクトが HttpContext.Items コレクションにアタッチされ、現在のリクエストのスコープ内でアクセスできるようになります。

トークンの検証が失敗した場合、あるいはトークンが存在しない場合は、HTTP コンテキストに認証済みユーザーオブジェクトがアタッチされていないため、リクエストはパブリック (匿名) ルートへのアクセスのみが許可されます。
ユーザーオブジェクトがアタッチされているかどうかを確認する認可ロジックは、custom authorize 属性にあり、認可に失敗した場合は 401 Unauthorized レスポンスを返します。


#### /Authorization/JwtUtils.cs


JWT utils クラスは、JWT トークンの生成と検証を行うためのメソッドを備えています。
GenerateToken()メソッドは、指定されたユーザーのIDを "id "クレームとしてJWTトークンを生成します。<userId> (例: "id": 1)を含むことになります。
ValidateToken() メソッドは、提供された JWT トークンの検証を試み、トークン請求の中からユーザー ID ("id") を返します。検証が失敗した場合は、null が返されます。