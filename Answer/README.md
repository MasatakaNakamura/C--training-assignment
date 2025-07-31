# 顧客管理システム - 模範解答

本プロジェクトは「C#研修課題：顧客管理アプリケーション開発」の模範解答です。

## 📋 プロジェクト概要

Windows Formsを使用した顧客管理デスクトップアプリケーションです。MVPアーキテクチャパターンを採用し、学習効果と実用性を両立した設計となっています。

### 技術スタック
- **言語**: C# (.NET 6)
- **UI**: Windows Forms
- **データベース**: MySQL
- **ORM**: Entity Framework Core 6
- **アーキテクチャ**: MVP (Model-View-Presenter) パターン

## 🏗️ プロジェクト構造

```
CustomerManager.sln
├── CustomerManager.WinForms/     # UI層（Windows Forms）
│   ├── Views/                   # フォーム（View）
│   │   ├── CustomerListView.cs        # 顧客一覧画面
│   │   └── CustomerEditView.cs        # 顧客登録・編集画面
│   ├── Presenters/              # プレゼンター層
│   │   ├── CustomerListPresenter.cs   # 一覧画面のPresenter
│   │   └── CustomerEditPresenter.cs   # 編集画面のPresenter
│   └── Program.cs               # エントリーポイント
├── CustomerManager.Core/         # ビジネスロジック層
│   ├── Models/                  # エンティティモデル
│   │   └── Customer.cs                # 顧客エンティティ
│   ├── Interfaces/              # インターフェース定義
│   │   ├── ICustomerRepository.cs     # データアクセス抽象化
│   │   ├── ICustomerListView.cs       # 一覧画面抽象化
│   │   └── ICustomerEditView.cs       # 編集画面抽象化
│   └── Services/                # ビジネスサービス
│       └── ValidationService.cs       # バリデーション機能
└── CustomerManager.Data/         # データアクセス層
    └── DataAccess/              # Entity Framework関連
        ├── CustomerDbContext.cs        # データベースコンテキスト
        └── CustomerRepository.cs      # データアクセス実装
```

## 🚀 セットアップ手順

### 1. 前提条件

以下のソフトウェアがインストールされている必要があります：

- Visual Studio 2022（または Visual Studio Code + .NET SDK 6.0）
- MySQL Server 8.0以降
- Git

### 2. データベースセットアップ

1. MySQLサーバーを起動
2. 提供されたSQLスクリプトを実行：

```bash
mysql -u root -p < database-setup.sql
```

### 3. プロジェクトセットアップ

1. プロジェクトをクローンまたはダウンロード
2. Visual Studioでソリューションファイル（`CustomerManager.sln`）を開く
3. 接続文字列を設定：
   - `CustomerManager.WinForms/appsettings.json` を編集
   - MySQL接続情報（サーバー、ユーザー名、パスワード）を設定

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=customer_management;User=root;Password=あなたのパスワード;"
  }
}
```

4. NuGetパッケージを復元（Visual Studioが自動実行）
5. スタートアッププロジェクトを `CustomerManager.WinForms` に設定
6. F5キーでデバッグ実行

## 🔧 主な機能

### コード品質向上機能
- ✅ **マジックストリング排除**: 定数クラスによるメッセージ・フィールド名の一元管理
- ✅ **リソース管理**: IDisposableパターンによる適切なメモリ管理
- ✅ **グローバル例外ハンドリング**: 未捕捉例外の一元処理とログ記録
- ✅ **安全なエンティティ更新**: 意図しないプロパティ更新の防止

### 顧客一覧画面
- ✅ 全顧客データの一覧表示（DataGridView）
- ✅ 新規登録ボタン
- ✅ 編集ボタン（選択した顧客の編集）
- ✅ 削除ボタン（確認ダイアログ付き）
- ✅ 更新ボタン（データ再読み込み）
- ✅ ダブルクリックで編集
- ✅ ローディング表示

### 顧客登録・編集画面
- ✅ 氏名、フリガナ、電話番号、メールアドレスの入力
- ✅ 必須項目バリデーション（氏名、メールアドレス）
- ✅ メールアドレス形式チェック
- ✅ メールアドレス重複チェック
- ✅ フィールド単位のエラー表示
- ✅ モーダルダイアログ表示
- ✅ キーボードショートカット（Ctrl+Enter: 保存, Escape: キャンセル）

### データベース機能
- ✅ CRUD操作の完全実装
- ✅ 作成日時・更新日時の自動設定
- ✅ トランザクション処理
- ✅ 接続エラーハンドリング

## 🎯 学習ポイント

### 1. コード品質とベストプラクティス
- **定数管理**: マジックストリングの排除による保守性向上
- **リソース管理**: IDisposableパターンによるメモリリーク防止
- **例外ハンドリング**: グローバル例外ハンドラによる堅牢性確保
- **データ更新の安全性**: Entity Frameworkでの適切な更新パターン

### 2. アーキテクチャパターン（MVP）
- **責務の分離**: View（表示）、Presenter（制御）、Model（データ）の明確な分離
- **テスタビリティ**: Presenterのロジック部分の単体テスト可能性
- **保守性**: 各層の独立性による変更影響の局所化

### 2. 依存性の注入（DI）
- インターフェースを通じた疎結合設計
- モック化によるテストの容易性
- 設定変更による実装切り替え

### 3. エラーハンドリング
- 適切な例外キャッチと処理
- ユーザーフレンドリなエラーメッセージ
- 予期せぬエラーへの防御的プログラミング

### 4. Entity Framework Core
- Database Firstアプローチ
- 自動的なSQLインジェクション対策
- 型安全なクエリ実行

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

### 非同期処理
- データベースアクセスは非同期処理（`async/await`）
- UIブロッキングの回避
- 適切なエラーハンドリング

## 🧪 テスト方針

### 単体テスト対象
- `CustomerListPresenter`のビジネスロジック
- `CustomerEditPresenter`のバリデーションロジック
- `ValidationService`の検証ルール
- `CustomerRepository`のデータアクセスロジック

### 統合テスト対象
- データベースとの接続・CRUD操作
- View-Presenter間の連携

## 📚 発展課題への対応

以下の機能を実装することで、さらなる学習効果が期待できます：

### 1. 検索機能
```csharp
// ICustomerRepositoryに追加
Task<IEnumerable<Customer>> SearchAsync(string keyword);
```

### 2. ページング機能
```csharp
// ページング用のパラメータクラス
public class PageRequest
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 20;
}
```

### 3. データのエクスポート
- CSV出力機能
- Excel出力機能（EPPlusライブラリ使用）

### 4. 設定管理
- ユーザー設定の永続化
- アプリケーション設定の外部化

## 🛡️ セキュリティ考慮事項

### 実装済み対策
- **SQLインジェクション対策**: Entity Frameworkによる自動的なパラメータ化
- **入力検証**: Data Annotationsとカスタムバリデーション
- **エラー情報の制御**: 詳細なエラー情報の非表示

### 本番環境での追加考慮事項
- **接続文字列の暗号化**: 機密情報の保護
- **ログ出力**: セキュリティイベントの記録
- **アクセス制御**: ユーザー認証・認可機能

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

### Entity Framework関連コマンド
```bash
# データベーススキーマの逆エンジニアリング（Database First）
dotnet ef dbcontext scaffold "ConnectionString" Pomelo.EntityFrameworkCore.MySql -o Models -c CustomerDbContext

# マイグレーション作成
dotnet ef migrations add InitialCreate --project CustomerManager.Data

# データベース更新
dotnet ef database update --project CustomerManager.Data
```

## 📝 今後の改善案

1. **ロギングシステムの導入**（NLog、Serilog）
2. **DIコンテナの導入**（Microsoft.Extensions.DependencyInjection）
3. **設定管理の強化**（Options パターン）
4. **国際化対応**（リソースファイルによる多言語対応）
5. **パフォーマンス最適化**（キャッシュ機能、遅延読み込み）

## 🤝 貢献・フィードバック

本模範解答に対するフィードバックや改善提案がございましたら、イシューまたはプルリクエストをお送りください。

---

**作成者**: Claude AI  
**最終更新**: 2025年7月31日  
**バージョン**: 1.0.0