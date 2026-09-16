using System;

// Essa é a classe abstrata para gravar os dados do universo em um arquivo txt.
public abstract class GravadorUniverso
{
    public abstract void Salvar(Universo universo, string caminho);
    public abstract Universo Carregar(string caminho);
}

public class GravadorArquivoTexto : GravadorUniverso
{
    public override void Salvar(Universo universo, string caminho)
    { // System.IO // }
    public override Universo Carregar(string caminho)
    { // System.IO // }
    }
}
