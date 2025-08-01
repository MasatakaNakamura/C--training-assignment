# Visual Studio 動作確認手順

## 🎯 概要

この顧客管理システムをVisual Studioで開発・実行するための詳細手順です。

## 📋 前提条件

### 必要なソフトウェア
- **Visual Studio 2022** (Community/Professional/Enterprise)
- **.NET 6.0 SDK** 以降
- **MySQL Server 8.0** 以降 (またはDocker)
- **Git** (ソースコード取得用)

## 🚀 セットアップ手順

### Step 1: プロジェクトの取得と展開

1. **ソースコードの配置**
   ```
   C:\Dev\CustomerManager\  (推奨パス)
   ├── CustomerManager.sln
   ├── CustomerManager.WinForms/
   ├── CustomerManager.Core/
   ├── CustomerManager.Data/
   └── CustomerManager.Console/
   ```

2. **Visual Studio起動**
   - Visual Studio 2022を起動
   - 「プロジェクトまたはソリューションを開く」を選択
   - `CustomerManager.sln`を開く

### Step 2: データベース準備

#### 🐳 Option A: Docker使用（推奨）

```bash
# PowerShellまたはコマンドプロンプトで実行
cd C:\Dev\CustomerManager
docker-compose -f docker-compose.dev.yml up -d
```

**メリット**: 環境構築が簡単、設定済み

#### 💾 Option B: ローカルMySQL使用

1. **MySQL Server インストール**
   - MySQL 8.0以降をインストール
   - ルートパスワードを設定

2. **データベース・テーブル作成**
   ```sql
   -- MySQL Workbenchまたはコマンドラインで実行
   CREATE DATABASE customer_management CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;
   
   USE customer_management;
   
   CREATE TABLE customers (
       id INT PRIMARY KEY AUTO_INCREMENT,
       name VARCHAR(255) NOT NULL,
       kana VARCHAR(255),
       phone_number VARCHAR(20),
       email VARCHAR(255) NOT NULL UNIQUE,
       created_at DATETIME,
       updated_at DATETIME
   );
   
   -- サンプルデータ挿入
   INSERT INTO customers (name, kana, phone_number, email, created_at, updated_at) VALUES
   ('田中太郎', 'タナカタロウ', '03-1234-5678', 'tanaka@example.com', NOW(), NOW()),
   ('佐藤花子', 'サトウハナコ', '090-1234-5678', 'sato@example.com', NOW(), NOW());
   ```

### Step 3: 接続文字列の設定

#### Docker使用の場合
`CustomerManager.WinForms/appsettings.json`を編集：

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=customer_management;User=customer_user;Password=customer_password;"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.EntityFrameworkCore": "Warning"
    }
  }
}
```

#### ローカルMySQL使用の場合
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=customer_management;User=root;Password=あなたのMySQLパスワード;"
  }
}
```

### Step 4: NuGetパッケージの復元

1. **自動復元の確認**
   - Visual Studioが自動的にパッケージを復元
   - 「出力」ウィンドウで進行状況を確認

2. **手動復元（必要時）**
   - ソリューションエクスプローラーでソリューションを右クリック
   - 「NuGetパッケージの復元」を選択

### Step 5: ビルドと実行

1. **ソリューション全体のビルド**
   ```
   メニュー: ビルド → ソリューションのビルド
   または: Ctrl+Shift+B
   ```

2. **スタートアッププロジェクトの設定**
   - ソリューションエクスプローラーで「CustomerManager.WinForms」を右クリック
   - 「スタートアッププロジェクトに設定」を選択

3. **アプリケーション実行**
   ```
   F5キー (デバッグ実行)
   または
   Ctrl+F5 (デバッグ無し実行)
   ```

## 🔧 開発時の操作確認

### アプリケーション起動後の動作テスト

#### 1. 顧客一覧画面の確認
- ✅ アプリケーション起動時にデータが表示される
- ✅ ボタンが正常に配置されている
- ✅ DataGridViewにサンプルデータが表示

#### 2. 新規登録機能
1. **「新規登録」ボタンクリック**
2. **顧客登録・編集画面が表示**
3. **データ入力テスト**
   ```
   氏名: 山田太郎
   フリガナ: ヤマダタロウ  
   電話番号: 080-1234-5678
   メールアドレス: yamada@test.com
   ```
4. **「登録」ボタンクリック**
5. **一覧画面に戻り、新しいデータが表示されることを確認**

#### 3. 編集機能
1. **一覧から顧客を選択**
2. **「編集」ボタンクリック**
3. **既存データが入力済みで表示**
4. **データを変更して「更新」ボタンクリック**
5. **変更が反映されることを確認**

#### 4. 削除機能
1. **一覧から顧客を選択**
2. **「削除」ボタンクリック**
3. **確認ダイアログが表示**
4. **「はい」選択**
5. **データが削除されることを確認**

#### 5. バリデーション確認
- **必須項目エラー**: 氏名・メール未入力時
- **重複エラー**: 既存メールアドレス入力時
- **フィールド別エラー表示**: 各項目の下にエラーメッセージ

## 🐛 トラブルシューティング

### よくある問題と解決方法

#### 1. データベース接続エラー
```
"データベースに接続できませんでした"
```

**確認項目**:
- [ ] MySQLサーバーが起動している
- [ ] 接続文字列が正しい
- [ ] データベース・テーブルが存在する
- [ ] ファイアウォール設定

**解決手順**:
```bash
# Docker使用の場合
docker-compose -f docker-compose.dev.yml ps
docker-compose -f docker-compose.dev.yml logs mysql-dev

# ローカルMySQL使用の場合
mysql -u root -p -e "SHOW DATABASES;"
```

#### 2. NuGetパッケージエラー
```
"パッケージが見つかりません"
```

**解決手順**:
1. `Tools → NuGet Package Manager → Package Manager Console`
2. 以下実行:
   ```powershell
   Update-Package -reinstall
   ```

#### 3. ビルドエラー
```
"プロジェクト参照が見つかりません"
```

**解決手順**:
1. ソリューションエクスプローラーで各プロジェクトの「依存関係」確認
2. 不足している参照を追加:
   ```
   プロジェクト右クリック → 追加 → プロジェクト参照
   ```

#### 4. アプリケーション起動エラー
```
"設定エラー" ダイアログ
```

**解決手順**:
1. `appsettings.json`の存在確認
2. 接続文字列の構文確認
3. ファイルのコピー設定確認:
   ```xml
   <Content Include="appsettings.json">
     <CopyToOutputDirectory>PreserveNewest</CopyToOutputDirectory>
   </Content>
   ```

### デバッグのベストプラクティス

#### 1. ブレークポイントの設定
- **Presenter**の各メソッドにブレークポイント設定
- データベース操作前後での変数確認

#### 2. 出力ウィンドウの活用
```
表示 → 出力 → 出力元: デバッグ
```

#### 3. 例外設定
```
デバッグ → 例外設定 → Common Language Runtime Exceptions
```

## 📊 パフォーマンステスト

### 大量データでのテスト

```sql
-- 大量テストデータ生成
INSERT INTO customers (name, kana, phone_number, email, created_at, updated_at)
SELECT 
    CONCAT('テスト顧客', n) as name,
    CONCAT('テストコキャク', n) as kana,
    CONCAT('080-', LPAD(n, 4, '0'), '-', LPAD(n*2, 4, '0')) as phone_number,
    CONCAT('test', n, '@example.com') as email,
    NOW() as created_at,
    NOW() as updated_at
FROM (
    SELECT a.N + b.N * 10 + c.N * 100 + 1 AS n
    FROM 
    (SELECT 0 AS N UNION ALL SELECT 1 UNION ALL SELECT 2 UNION ALL SELECT 3 UNION ALL SELECT 4 UNION ALL SELECT 5 UNION ALL SELECT 6 UNION ALL SELECT 7 UNION ALL SELECT 8 UNION ALL SELECT 9) a,
    (SELECT 0 AS N UNION ALL SELECT 1 UNION ALL SELECT 2 UNION ALL SELECT 3 UNION ALL SELECT 4 UNION ALL SELECT 5 UNION ALL SELECT 6 UNION ALL SELECT 7 UNION ALL SELECT 8 UNION ALL SELECT 9) b,
    (SELECT 0 AS N UNION ALL SELECT 1 UNION ALL SELECT 2 UNION ALL SELECT 3 UNION ALL SELECT 4 UNION ALL SELECT 5 UNION ALL SELECT 6 UNION ALL SELECT 7 UNION ALL SELECT 8 UNION ALL SELECT 9) c
) numbers
WHERE n <= 1000;
```

### 応答性確認項目
- [ ] 1000件データでの起動時間 < 3秒
- [ ] 検索・フィルタリング応答 < 1秒
- [ ] CRUD操作応答 < 500ms

## 🎓 学習ポイント

### Visual Studio活用テクニック

1. **IntelliSense活用**
   - Ctrl+スペース: 自動補完
   - F12: 定義へ移動
   - Shift+F12: 参照検索

2. **リファクタリング**
   - F2: 名前変更
   - Ctrl+R, Ctrl+M: メソッド抽出

3. **デバッグ技術**
   - F9: ブレークポイント
   - F10: ステップオーバー
   - F11: ステップイン

## 📝 開発ワークフロー

### 日常的な開発サイクル

1. **起動時チェック**
   ```bash
   docker-compose -f docker-compose.dev.yml ps
   ```

2. **Visual Studio起動**
   - ソリューション読み込み
   - 最新コードの取得（Git）

3. **開発・テスト**
   - 機能実装
   - ローカルテスト実行
   - デバッグ

4. **終了時**
   - 変更のコミット
   - データベースコンテナ停止（必要に応じて）

---

この手順に従って、Visual Studioでの開発環境を整備し、顧客管理システムの動作確認を行うことができます。