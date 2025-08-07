#ifdef _WIN32
#    define NOINLINE __declspec(noinline)
#else
#    define NOINLINE __attribute__((noinline))
#endif

NOINLINE void save_score_to_disk(int score)
{
    char *ptr = 0;
    *ptr += score;
}
