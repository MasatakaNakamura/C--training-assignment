# C#研修課題：顧客管理アプリケーション開発

本リポジトリは、C#/.NET技術を学習するための実践的な研修課題プロジェクトです。Windows Formsを使用した顧客管理デスクトップアプリケーションの開発を通じて、エンタープライズレベルの開発スキルを習得することを目的としています。

## 📚 課題概要

### 目標
- **C#/.NET技術の習得**: .NET 8、Entity Framework Core、MVPパターンの実践
- **データベース連携**: MySQLとの統合、CRUD操作の実装
- **企業レベル品質**: 依存性注入、ログ機能、例外ハンドリングの実装
- **テスト駆動開発**: 単体テスト、統合テストの作成

### 技術スタック
- **言語**: C# (.NET 8)
- **UI**: Windows Forms
- **データベース**: MySQL 8.0+
- **ORM**: Entity Framework Core 8
- **アーキテクチャ**: MVP (Model-View-Presenter) パターン
- **ログ**: NLog
- **DI**: Microsoft.Extensions.DependencyInjection
- **テスト**: xUnit + FluentAssertions + Moq

## 🏗️ プロジェクト構成

```
C--training-assignment/
├── docs/                           # 課題ドキュメント
│   └── training-assignment.md           # 詳細な課題仕様書
├── Answer/                         # 実装版（完成版サンプル）
│   ├── CustomerManager.sln              # ソリューションファイル
│   ├── CustomerManager.WinForms/         # UI層（Windows Forms）
│   ├── CustomerManager.Core/             # ビジネスロジック層
│   ├── CustomerManager.Data/             # データアクセス層
│   ├── CustomerManager.Tests/            # テストプロジェクト
│   ├── CustomerManager.Console/          # コンソールアプリケーション
│   ├── database-setup.sql               # データベース初期化スクリプト
│   └── README.md                        # 実装版ドキュメント
├── CLAUDE.md                       # Claude AI向け開発ガイド
├── .gitignore                      # Git除外設定
└── README.md                       # このファイル
```

## 🎯 学習目標と達成レベル

### レベル1: 基本実装
- [x] **プロジェクト作成**: Visual Studioプロジェクトの作成
- [x] **データベース設計**: customers テーブルの作成
- [x] **Entity Framework**: Code First または Database First の実装
- [x] **CRUD機能**: 顧客情報の登録・表示・更新・削除

### レベル2: アーキテクチャ実装
- [x] **MVPパターン**: View、Presenter、Modelの分離
- [x] **Repository パターン**: データアクセスの抽象化
- [x] **バリデーション**: Data Annotations による入力検証
- [x] **例外ハンドリング**: グローバル例外ハンドラの実装

### レベル3: エンタープライズ機能
- [x] **依存性注入**: DIコンテナの活用
- [x] **ログ機能**: NLogによる構造化ログ
- [x] **設定管理**: appsettings.json による外部化
- [x] **非同期処理**: async/await の適切な実装

### レベル4: テストとCI/CD
- [ ] **単体テスト**: xUnit + Moq によるテスト作成
- [ ] **統合テスト**: Entity Framework InMemory を使用
- [ ] **コードカバレッジ**: 80%以上のカバレッジ達成
- [ ] **CI/CD**: GitHub Actions による自動ビルド・テスト

## 🚀 開始方法

### 1. 前提条件
以下のソフトウェアが必要です：
- Visual Studio 2022 または Visual Studio Code + .NET 8 SDK
- MySQL Server 8.0+ または Docker
- Git

### 2. 課題仕様の確認
```bash
# リポジトリをクローン
git clone <repository-url>
cd C--training-assignment

# 課題仕様書を確認
docs/training-assignment.md を参照
```

### 3. 実装開始
```bash
# 新しいブランチを作成
git checkout -b feature/my-implementation

# プロジェクト作成（例）
dotnet new sln -n CustomerManager
dotnet new winforms -n CustomerManager.WinForms
dotnet new classlib -n CustomerManager.Core
dotnet new classlib -n CustomerManager.Data
dotnet new xunit -n CustomerManager.Tests
```

### 4. 参考実装の確認
完成版のサンプル実装が `Answer/` ディレクトリに含まれています：
```bash
cd Answer
# Answer/README.md で詳細な実装内容を確認
```

## 📋 必須機能要件

### 顧客一覧画面
- ✅ DataGridView による顧客データ表示
- ✅ 新規登録ボタン
- ✅ 編集ボタン（選択した顧客の編集）
- ✅ 削除ボタン（確認ダイアログ付き）
- ✅ 更新ボタン（データ再読み込み）

### 顧客登録・編集画面
- ✅ 氏名、フリガナ、電話番号、メールアドレスの入力
- ✅ 必須項目バリデーション（氏名、メールアドレス）
- ✅ メールアドレス形式チェック
- ✅ メールアドレス重複チェック
- ✅ モーダルダイアログ表示

### データベース機能
- ✅ CRUD操作の完全実装
- ✅ 作成日時・更新日時の自動設定
- ✅ トランザクション処理
- ✅ 接続エラーハンドリング

## 🛠️ データベース設計

### customers テーブル
```sql
CREATE TABLE customers (
    id INT PRIMARY KEY AUTO_INCREMENT,
    name VARCHAR(255) NOT NULL COMMENT '氏名（必須）',
    kana VARCHAR(255) COMMENT 'フリガナ',
    phone_number VARCHAR(20) COMMENT '電話番号',
    email VARCHAR(255) NOT NULL UNIQUE COMMENT 'メールアドレス（必須・一意）',
    created_at DATETIME COMMENT '作成日時',
    updated_at DATETIME COMMENT '更新日時'
);
```

## 🧪 テスト戦略

### テスト種別
1. **単体テスト**
   - ValidationService のテスト
   - CustomerRepository のテスト
   - Presenter のテスト

2. **統合テスト**
   - データベース操作のテスト
   - API エンドポイントのテスト

3. **UI テスト**
   - フォーム表示のテスト
   - ユーザー操作のテスト

### テスト実行
```bash
# 全テスト実行
dotnet test

# カバレッジ付きテスト
dotnet test --collect:"XPlat Code Coverage"
```

## 🎯 学習ポイント

### 1. アーキテクチャパターン
- **MVPパターン**: 責務分離の実践
- **Repository パターン**: データアクセスの抽象化
- **依存性注入**: DIコンテナによる疎結合設計

### 2. データアクセス技術
- **Entity Framework Core**: O/Rマッピングの活用
- **非同期処理**: async/await によるパフォーマンス向上
- **トランザクション**: データ整合性の保証

### 3. エラーハンドリング
- **グローバル例外ハンドラ**: 未捕捉例外の一元管理
- **Data Annotations**: 宣言的バリデーション
- **ユーザーフレンドリ**: エンドユーザー向けエラーメッセージ

### 4. コード品質
- **定数クラス**: マジックストリングの排除
- **Nullable参照型**: null安全性の向上
- **IDisposable**: リソース管理の徹底

## 📊 評価基準

### 機能実装 (40%)
- [x] 基本CRUD機能の動作
- [x] バリデーション機能
- [x] UI/UXの使いやすさ
- [x] エラーハンドリング

### コード品質 (30%)
- [x] アーキテクチャパターンの適用
- [x] 命名規約の遵守
- [x] コメント・ドキュメント
- [x] リファクタリングの実施

### テスト (20%)
- [ ] 単体テストのカバレッジ
- [ ] 統合テストの実装
- [ ] テストコードの品質

### 技術的挑戦 (10%)
- [x] 非同期処理の活用
- [x] 依存性注入の実装
- [x] ログ機能の実装
- [ ] CI/CDの構築

## 🔧 開発環境セットアップ

### Visual Studio 2022
1. Visual Studio 2022 をインストール
2. .NET 8 SDK を確認
3. MySQL Workbench をインストール（推奨）

### Visual Studio Code
```bash
# .NET 8 SDK インストール
# https://dotnet.microsoft.com/download

# 必要な拡張機能
code --install-extension ms-dotnettools.csharp
code --install-extension ms-dotnettools.vscode-dotnet-runtime
```

### MySQL環境
```bash
# Docker を使用する場合
docker run --name mysql-dev -e MYSQL_ROOT_PASSWORD=rootpassword -e MYSQL_DATABASE=customer_management -p 3306:3306 -d mysql:8.0

# 直接インストールする場合
# https://dev.mysql.com/downloads/mysql/
```

## 📖 参考資料

### 公式ドキュメント
- [.NET 8 Documentation](https://docs.microsoft.com/dotnet/)
- [Entity Framework Core](https://docs.microsoft.com/ef/)
- [Windows Forms](https://docs.microsoft.com/dotnet/desktop/winforms/)

### デザインパターン
- [MVP Pattern](https://docs.microsoft.com/previous-versions/msp-n-p/ff649571(v=pandp.10))
- [Repository Pattern](https://docs.microsoft.com/dotnet/architecture/microservices/microservice-ddd-cqrs-patterns/infrastructure-persistence-layer-design)

### テスト
- [xUnit Documentation](https://xunit.net/)
- [Moq Documentation](https://github.com/moq/moq4)

## 🤝 サポート・質問

### 質問方法
1. **GitHub Issues**: バグレポート、機能要求
2. **GitHub Discussions**: 実装方針、技術的な質問
3. **Pull Request**: コードレビュー依頼

### メンター制度
- 週1回のコードレビュー
- 困った時のペアプログラミング
- アーキテクチャ設計の相談

## 📈 進捗管理

### マイルストーン
- **Week 1**: プロジェクト作成、データベース設計
- **Week 2**: 基本CRUD機能の実装
- **Week 3**: MVPパターンの適用、バリデーション
- **Week 4**: テスト実装、コードレビュー

### 完了チェックリスト
- [ ] データベースが正常に作成・接続できる
- [ ] 顧客の新規登録ができる
- [ ] 顧客一覧が表示される
- [ ] 顧客情報の編集ができる
- [ ] 顧客の削除ができる（確認ダイアログ付き）
- [ ] バリデーションが適切に動作する
- [ ] エラーハンドリングが実装されている
- [ ] 単体テストが作成されている
- [ ] コードが適切にコメントされている

---

**研修責任者**: Development Team  
**最終更新**: 2025年8月2日  
**対象フレームワーク**: .NET 8.0  
**データベース**: MySQL 8.0+  
**推定期間**: 4週間（80時間）