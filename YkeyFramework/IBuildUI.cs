using System;

namespace YK;

public interface IBuildUI<T, TA> where T : ELayer where TA : IBuildUIArgs
{
    T Setup(TA args);
}

