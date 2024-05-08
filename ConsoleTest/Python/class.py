from jinja2 import Template

# 定义模板字符串
template_a = """
/// <summary>
/// {{alpha}}语句
/// {{A}}  -> {{beta}} {{A}}'
/// {{A}}' -> {{alpha}} {{beta}} {{A}}'
///          | ε
/// </summary>
/// <returns></returns>
void {{A}}(TreeNode root)
{
    {{beta}}(root.AddChild("{{beta}}"));
    {{A}}_(root.AddChild("{{A}}'"));
}

void {{A}}_(TreeNode root)
{
    if (look.tag == {{tag}})
    {
        Match({{tag}});
        root.AddChild("{{alpha}}");
        {{beta}}(root.AddChild("{{beta}}"));
        {{A}}_(root.AddChild("{{A}}'"));
    }
    else
    {
        root.AddChild("empty");
    }
}

"""


template_b = """
/// <summary>
/// {{alpha}}语句
/// {{A}}  -> {{beta}} {{A}}'
/// {{A}}' -> {{alpha1}} {{beta}} {{A}}'
///          | {{alpha2}} {{beta}} {{A}}'
///          | ε
/// </summary>
/// <returns></returns>
void {{A}}(TreeNode root)
{
    {{beta}}(root.AddChild("{{beta}}"));
    {{A}}_(root.AddChild("{{A}}'"));
}

void {{A}}_(TreeNode root)
{
    if (look.tag == {{tag1}} || look.tag == {{tag2}})
    {
        Word w = (Word)look;
        Move();
        root.AddChild(w.lexeme);
        {{beta}}(root.AddChild("{{beta}}"));
        {{A}}_(root.AddChild("{{A}}'"));
    }
    else
    {
        root.AddChild("empty");
    }
}

"""



template_c = """
/// <summary>
/// {{alpha}}语句
/// {{A}}  -> {{beta}} {{A}}'
/// {{A}}' -> {{alpha1}} {{beta}} {{A}}'
///          | {{alpha2}} {{beta}} {{A}}'
///          | {{alpha3}} {{beta}} {{A}}'
///          | {{alpha4}} {{beta}} {{A}}'
///          | ε
/// </summary>
/// <returns></returns>
void {{A}}(TreeNode root)
{
    {{beta}}(root.AddChild("{{beta}}"));
    {{A}}_(root.AddChild("{{A}}'"));
}

void {{A}}_(TreeNode root)
{
    if (look.tag == {{tag1}} || look.tag == {{tag2}} || look.tag == {{tag3}} || look.tag == {{tag4}})
    {
        Word w = (Word)look;
        Move();
        root.AddChild(w.lexeme);
        {{beta}}(root.AddChild("{{beta}}"));
        {{A}}_(root.AddChild("{{A}}'"));
    }
    else
    {
        root.AddChild("empty");
    }
}

"""


template_d = """
/// <summary>
/// {{alpha}}语句
/// {{A}} -> {{alpha1}}  {{A}}
///          | {{alpha2}}  {{A}}
///          | {{beta}}
/// </summary>
/// <returns></returns>
void {{A}}(TreeNode root)
{
    if (look.tag == {{tag1}} || look.tag == {{tag2}})
    {
        Word w = (Word)look;
        Move();
        root.AddChild(w.lexeme);
        {{A}}(root.AddChild("{{A}}"));
    }
    else
    {
        {{beta}}(root.AddChild("{{beta}}"));
    }
}

"""



# 创建模板对象
templatea = Template(template_a)
templateb = Template(template_b)
templatec = Template(template_c)
templated = Template(template_d)

with open('code.txt', 'w', encoding='utf-8') as file:
    generated_code = templatea.render(A = "Bool", beta = "Join", tag = "Tag.OR", alpha = "||")
    file.write(generated_code)
    generated_code = templatea.render(A = "Join", beta = "Equality", tag = "Tag.AND", alpha = "&&")
    file.write(generated_code)
    generated_code = templateb.render(A = "Equality", beta = "Compare", tag1 = "Tag.EQ", tag2 = 'Tag.NE', alpha1 = "==", alpha2 = "!=")
    file.write(generated_code)
    generated_code = templatec.render(A = "Compare", beta = "Expr", \
                                      tag1 = "'<'", tag2 = "Tag.LT", tag3 = "'>'", tag4 = "Tag.GT", \
                                      alpha1 = "<", alpha2 = "<=", alpha3 = ">", alpha4 = ">=")
    file.write(generated_code)
    generated_code = templateb.render(A = "Expr", beta = "Term", tag1 = "'+'", tag2 = "'-'", alpha1 = "+", alpha2 = "-")
    file.write(generated_code)
    generated_code = templateb.render(A = "Term", beta = "Unary", tag1 = "'*'", tag2 = "'/'", alpha1 = "*", alpha2 = "/")
    file.write(generated_code)
    generated_code = templated.render(A = "Unary", beta = "Factor", tag1 = "'!'", tag2 = "'-'", alpha1 = "!", alpha2 = "-")
    file.write(generated_code)

   
