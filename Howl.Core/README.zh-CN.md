# HowlCore

[English](README.md)

一个基于 .NET Standard 2.1 的类库，为 Howl 框架提供通用工具、扩展方法和设计模式。

## 功能特性

### Result 模式
一套稳健的操作结果模式，用于API响应和服务操作：
- `Result` / `Result<T>` - 封装操作状态、消息、错误代码和数据
- `ResultBuilder` - 用于创建成功/失败结果的静态工厂方法

### 分页支持
内置分页查询支持：
- `Query<T>` - 分页参数（页码、每页数量、跳过数量、排序、关联实体）
- `Paged<T>` - 分页结果容器，包含总数和数据项

### 扩展方法
丰富的扩展方法集，涵盖常见操作：

**类型转换**
- `To<T>()` - 安全类型转换，支持默认值
- `Nullable<T>()` - 转换为可空类型
- `As<T>()` - 尝试转换为目标类型

**字符串操作**
- `EqualsIgnoreCase()` / `EqualsFully()` - 字符串比较辅助方法
- `SplitString()` / `SplitTo<T>()` - 字符串分割并转换类型
- `ToQueryString()` / `AppendQueries()` - URL查询字符串处理
- `Coalesce()` - 返回第一个非空字符串

**集合操作**
- `WhereIf()` - 条件LINQ筛选
- `IsAny()` / `IsEmpty()` - 空值安全的集合判断
- `StringJoin()` - 将集合元素连接为字符串
- `AsList()` - 高效转换为List

**数字操作**
- `Between()` - 范围判断
- `Max()` / `Min()` - 值比较
- `ToFixed()` / `ToFixedAsString()` / `ToFixedFormat()` - 小数精度处理和格式化

**对象映射**
- `Map<T>()` - 使用AutoMapper自动映射对象
- `Inherit<T1, T2>()` - 从源对象复制属性到目标对象

**JSON序列化**
- `TrySerialize<T>()` / `TryDeserialize<T>()` - 使用Newtonsoft.Json的安全JSON操作

### 工厂模式
简单的工厂抽象：
- `IFactory<T>` - 工厂接口
- `DelegateFactory<T>` - 基于委托的工厂实现

### 异常类
自定义异常类型：
- `PlatformException` - 平台异常，包含错误代码和附加数据
- `HttpStatusCodeException` - HTTP状态码异常
- `BadRequestException` - 400 Bad Request异常

### 反射辅助
使用表达式树的高效反射工具：
- `Get()` / `Set()` - 动态属性访问
- `ToDictionary()` - 将对象转换为字典
- 表达式树解析成员访问路径

## 依赖

| 包 | 版本 | 用途 |
|---|---|---|
| AutoMapper | 9.0.0 | 对象映射 |
| Newtonsoft.Json | 13.0.4 | JSON序列化 |
| System.ComponentModel.Annotations | 5.0.0 | 数据注解 |

## 安装

通过NuGet安装：

```bash
dotnet add package Howl.Core
```

## 使用示例

### Result 模式

```csharp
// 成功结果
var result = ResultBuilder.Succeed(data, "操作完成");

// 失败结果
var failResult = ResultBuilder.Fail("输入无效");
var notFoundResult = ResultBuilder.NotFound("用户");

// 在异步方法中使用
public async Task<Result<User>> GetUser(int id)
{
    var user = await _repository.FindById(id);
    if (user == null)
        return ResultBuilder.NotFound("用户");
    return user; // 隐式转换为 Result<User>
}
```

### 分页

```csharp
public async Task<Paged<User>> GetUsers(Query<User> query)
{
    var total = await _context.Users.CountAsync();
    var items = await _context.Users
        .Skip(query.Skip ?? 0)
        .Take(query.Take ?? 10)
        .ToListAsync();
    return Paged<User>.Create(items, total);
}
```

### 扩展方法

```csharp
// 类型转换
var age = "25".To<int>(); // 25
var enabled = "true".To<bool>(); // true

// 条件筛选
var filtered = users.WhereIf(u => u.Active, isActive);

// 字符串操作
var query = parameters.ToQueryString();
var url = "https://api.example.com".AppendQueries(new { page = 1, size = 10 });

// 对象映射
var dto = entity.Map<UserDto>();
```

## 许可证

MIT License - Copyright 2020 hflhhb

## 仓库

[GitHub](https://github.com/hflhhb/HowlCore)
