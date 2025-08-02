# 顧客管理システム

本プロジェクトは「C#研修課題：顧客管理アプリケーション開発」の実装版です。MVPアーキテクチャパターンを採用した、学習効果の高いサンプル実装となっています。

## 📋 プロジェクト概要

Windows Formsを使用した顧客管理デスクトップアプリケーションです。MVPアーキテクチャパターンを採用し、学習効果と実用性を両立した設計となっています。

### 技術スタック
- **言語**: C# (.NET 6)
- **UI**: Windows Forms
- **データベース**: MySQL 8.0+
- **ORM**: Entity Framework Core 6 (Pomelo.EntityFrameworkCore.MySql)
- **アーキテクチャ**: MVP (Model-View-Presenter) パターン
- **設定管理**: Microsoft.Extensions.Configuration

## 🏗️ プロジェクト構造

```
CustomerManager.sln
├── CustomerManager.WinForms/     # UI層（Windows Forms）
│   ├── Views/                   # フォーム（View）
│   │   ├── CustomerListView.cs        # 顧客一覧画面
│   │   ├── CustomerListView.Designer.cs   # 一覧画面デザイナー
│   │   ├── CustomerEditView.cs        # 顧客登録・編集画面
│   │   └── CustomerEditView.Designer.cs   # 編集画面デザイナー
│   ├── Presenters/              # プレゼンター層
│   │   ├── CustomerListPresenter.cs   # 一覧画面のPresenter
│   │   └── CustomerEditPresenter.cs   # 編集画面のPresenter
│   ├── Program.cs               # エントリーポイント
│   ├── appsettings.json         # 設定ファイル
│   └── appsettings.Development.json   # 開発環境設定
├── CustomerManager.Core/         # ビジネスロジック層
│   ├── Models/                  # エンティティモデル
│   │   └── Customer.cs                # 顧客エンティティ
│   ├── Interfaces/              # インターフェース定義
│   │   ├── ICustomerRepository.cs     # データアクセス抽象化
│   │   ├── ICustomerListView.cs       # 一覧画面抽象化
│   │   └── ICustomerEditView.cs       # 編集画面抽象化
│   ├── Services/                # ビジネスサービス
│   │   └── ValidationService.cs       # バリデーション機能
│   └── Constants/               # 定数管理
│       ├── FieldConstants.cs          # フィールド名定数
│       └── MessageConstants.cs        # メッセージ定数
├── CustomerManager.Data/         # データアクセス層
│   ├── DataAccess/              # Entity Framework関連
│   │   ├── CustomerDbContext.cs        # データベースコンテキスト
│   │   └── CustomerRepository.cs      # データアクセス実装
│   └── Migrations/              # データベースマイグレーション
├── CustomerManager.Tests/        # テストプロジェクト（準備中）
│   └── Presenters/              # プレゼンターテスト用
├── database-setup.sql           # データベース初期化スクリプト
└── logs/                        # ログファイル格納用
    └── mysql/                   # MySQL関連ログ
```

## 🚀 セットアップ手順

### 1. 前提条件

以下のソフトウェアがインストールされている必要があります：

- Visual Studio 2022（または Visual Studio Code + .NET SDK 6.0以上）
- MySQL Server 8.0以降
- Git

### 2. データベースセットアップ

#### 方法1: 手動セットアップ
```bash
mysql -u root -p < database-setup.sql
```

#### 方法2: 手動データベース操作
```sql
-- MySQLにログインしてデータベースとテーブルを作成
CREATE DATABASE customer_management CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;
USE customer_management;

-- customersテーブルの作成
CREATE TABLE customers (
    id INT PRIMARY KEY AUTO_INCREMENT,
    name VARCHAR(255) NOT NULL,
    kana VARCHAR(255),
    phone_number VARCHAR(20),
    email VARCHAR(255) NOT NULL UNIQUE,
    created_at DATETIME,
    updated_at DATETIME
);
```

### 3. プロジェクトセットアップ

1. プロジェクトをクローンまたはダウンロード
2. Visual Studioでソリューションファイル（`CustomerManager.sln`）を開く
3. 接続文字列を設定：
   - `CustomerManager.WinForms/appsettings.json` を編集
   - MySQL接続情報を設定

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=customer_management;User=root;Password=your_password_here;"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.EntityFrameworkCore": "Warning"
    }
  }
}
```

4. NuGetパッケージを復元（Visual Studioが自動実行）
5. スタートアッププロジェクトを `CustomerManager.WinForms` に設定
6. F5キーでデバッグ実行

## 🔧 主な機能

### アーキテクチャ機能
- ✅ **MVPパターン**: Model-View-Presenterによる責務分離
- ✅ **設定外部化**: appsettings.json による設定管理
- ✅ **Entity Framework Core**: Pomelo MySQLプロバイダーによるO/Rマッピング
- ✅ **インターフェース分離**: テスタブルな設計

### コード品質向上機能
- ✅ **マジックストリング排除**: 定数クラスによるメッセージ・フィールド名の一元管理
- ✅ **リソース管理**: IDisposableパターンによる適切なメモリ管理
- ✅ **グローバル例外ハンドリング**: 未捕捉例外の一元処理とログ記録
- ✅ **厳密な型チェック**: Nullable参照型による null安全性
- ✅ **非同期処理**: async/await による適切な非同期実装

### 顧客一覧画面
- ✅ 全顧客データの一覧表示（DataGridView）
- ✅ 新規登録ボタン
- ✅ 編集ボタン（選択した顧客の編集）
- ✅ 削除ボタン（確認ダイアログ付き）
- ✅ 更新ボタン（データ再読み込み）
- ✅ 選択状態による動的ボタン制御

### 顧客登録・編集画面
- ✅ 氏名、フリガナ、電話番号、メールアドレスの入力
- ✅ 必須項目バリデーション（氏名、メールアドレス）
- ✅ メールアドレス形式チェック（Data Annotations）
- ✅ メールアドレス重複チェック（編集時は自分自身を除外）
- ✅ フィールド単位のエラー表示
- ✅ モーダルダイアログ表示

### データベース機能
- ✅ CRUD操作の完全実装
- ✅ 作成日時・更新日時の自動設定
- ✅ 接続エラーハンドリング
- ✅ メールアドレス一意制約

## 🧪 テスト機能

### テスト構成
現在、テストプロジェクトの準備を進めており、以下のテストフレームワークの使用を想定しています：

- **テストフレームワーク**: xUnit
- **アサーション**: FluentAssertions
- **モック**: Moq
- **データベーステスト**: Entity Framework InMemory

### 今後実装予定のテスト

#### ValidationServiceTests
- 正常系: 有効な顧客データ、最小限の必須項目
- 名前検証: 空文字、null、長すぎる名前
- メール検証: 空文字、null、無効な形式
- 電話番号検証: 無効な形式、有効な形式パターン
- 複合エラー: 複数フィールドの同時エラー

#### CustomerRepositoryTests
- CRUD操作: GetAll、GetById、Add、Update、Delete
- メール重複チェック: 存在確認、除外ID指定
- エラーハンドリング: null引数、存在しないID
- タイムスタンプ: 自動作成・更新日時設定

### テスト実行方法
```bash
# テストプロジェクトの作成が完了後
dotnet test CustomerManager.Tests
```

## 📊 ログ機能

### 現在の実装
現在は基本的なログ機能を実装しており、以下の機能を提供しています：

- **開発時**: Visual Studio出力ウィンドウ、コンソール出力
- **エラーログ**: グローバル例外ハンドラによる例外情報の記録

### 今後の拡張予定
- NLogの導入によるファイル出力
- ログレベル分け（Debug、Info、Warning、Error）
- ログローテーション機能

## 🐳 Docker対応

### 現在の状況
現在はローカル開発環境での実行を前提とした構成となっています。

### 今後の対応予定
- Docker Composeによる開発環境の提供
- MySQLコンテナとの連携
- クロスプラットフォーム対応（.NET 6の活用）

## 🎯 学習ポイント

### 1. アーキテクチャパターン学習
- **MVPパターン**: 責務分離の実践
- **設定管理**: appsettings.jsonによる外部化
- **例外ハンドリング**: グローバル例外ハンドラの実装
- **非同期処理**: async/awaitの適切な使用

### 2. データアクセスパターン
- **Repository パターン**: データアクセスの抽象化
- **Entity Framework Core**: O/Rマッピングとクエリ最適化
- **Pomelo MySQLプロバイダー**: MySQL特化の実装

### 3. エラーハンドリング戦略
- **グローバル例外ハンドラ**: 未捕捉例外の一元管理
- **Data Annotations**: 宣言的バリデーション
- **ユーザーフレンドリ**: エンドユーザー向けエラーメッセージ

### 4. コード品質向上
- **定数クラス**: マジックストリングの排除
- **Nullable参照型**: null安全性の向上
- **IDisposable**: リソース管理の徹底

## 🔍 コード品質の特徴

### 命名規約
- クラス名：PascalCase（例：`CustomerRepository`）
- メソッド名：PascalCase（例：`GetAllAsync`）
- プロパティ名：PascalCase（例：`CustomerName`）
- フィールド名：camelCase with underscore（例：`_repository`）

### 設計原則
- **単一責任原則（SRP）**: 各クラスは一つの責務のみ
- **開放閉鎖原則（OCP）**: インターフェースを通じた拡張性
- **依存性逆転原則（DIP）**: 具象クラスではなく抽象に依存
- **DRY原則**: 定数クラスによる重複排除

### 非同期処理
- データベースアクセスは非同期処理（`async/await`）
- UIブロッキングの回避

## 🛡️ セキュリティ考慮事項

### 実装済み対策
- **SQLインジェクション対策**: Entity Frameworkによる自動的なパラメータ化
- **入力検証**: Data Annotationsによる宣言的検証
- **メール形式検証**: EmailAddressAttributeによる検証
- **エラー情報の制御**: ユーザーフレンドリなエラーメッセージ
- **リソース管理**: IDisposableによる適切なクリーンアップ

### 本番環境での追加考慮事項
- **接続文字列の暗号化**: 機密情報の保護
- **監査ログ**: セキュリティイベントの記録
- **アクセス制御**: ユーザー認証・認可機能
- **通信暗号化**: HTTPS/TLS接続

## 🔧 開発者向け情報

### ビルドコマンド
```bash
# ソリューション全体のビルド
dotnet build CustomerManager.sln

# リリースビルド
dotnet build CustomerManager.sln -c Release

# 実行
dotnet run --project CustomerManager.WinForms
```

### テストコマンド
```bash
# 全テスト実行
dotnet test

# 特定プロジェクトのテスト
dotnet test CustomerManager.Tests

# カバレッジ付きテスト
dotnet test --collect:"XPlat Code Coverage"
```

### Entity Framework関連コマンド
```bash
# マイグレーション作成
dotnet ef migrations add InitialCreate --project CustomerManager.Data --startup-project CustomerManager.WinForms

# データベース更新
dotnet ef database update --project CustomerManager.Data --startup-project CustomerManager.WinForms

# データベーススキーマの逆エンジニアリング
dotnet ef dbcontext scaffold "Server=localhost;Database=customer_management;User=root;Password=your_password;" Pomelo.EntityFrameworkCore.MySql --project CustomerManager.Data
```

## 📈 パフォーマンス最適化

### 実装済み最適化
- **非同期処理**: UIブロッキング回避
- **接続管理**: DbContextの適切なライフサイクル管理
- **クエリ最適化**: Entity Frameworkによる効率的なクエリ
- **メモリ管理**: IDisposableによるリソース解放

### さらなる最適化案
- **キャッシュ機能**: 頻繁にアクセスされるデータのキャッシュ
- **ページング**: 大量データの段階的読み込み
- **インデックス最適化**: データベースパフォーマンス向上
- **クエリ分析**: 実行計画の最適化

## 📝 今後の改善案

### 機能追加
1. **検索・フィルタ機能**: 名前、メールアドレスによる検索
2. **データエクスポート**: CSV、Excel出力機能
3. **インポート機能**: 一括データ登録
4. **監査ログ**: データ変更履歴の記録
5. **レポート機能**: 各種統計レポート生成

### 技術的改善
1. **テスト実装**: xUnit、FluentAssertions、Moqを使用した包括的テスト
2. **ログ機能強化**: NLogによる本格的なログ機能
3. **Docker対応**: 開発環境のコンテナ化
4. **国際化対応**: リソースファイルによる多言語対応
5. **Web API化**: RESTful APIによるマルチクライアント対応

### 開発環境改善
1. **CI/CD パイプライン**: GitHub Actions による自動テスト・デプロイ
2. **コード品質ゲート**: 静的解析ツールの導入
3. **パフォーマンステスト**: 負荷テストの実装
4. **セキュリティスキャン**: 脆弱性検査の導入

## 🤝 貢献・フィードバック

本プロジェクトに対するフィードバックや改善提案がございましたら、以下の方法でご連絡ください：

- **Issue**: バグレポート、機能要求
- **Pull Request**: コード改善、新機能追加
- **Discussion**: 設計方針、技術的な議論

### 開発参加時の注意事項
- コーディング規約の遵守
- テストコードの作成
- ドキュメントの更新
- ログ出力の適切な実装

---

**作成者**: Claude AI  
**最終更新**: 2025年8月2日  
**バージョン**: 1.0.0 - Basic Implementation  
**ライセンス**: MIT License  
**対象フレームワーク**: .NET 6.0  
**データベース**: MySQL 8.0+