if(NOT TARGET games-frame-pacing::swappy)
add_library(games-frame-pacing::swappy SHARED IMPORTED)
set_target_properties(games-frame-pacing::swappy PROPERTIES
    IMPORTED_LOCATION "/Users/bitfox/.gradle/caches/transforms-3/2f34eafaa5072f0d6f8b0bc1d6fbf165/transformed/jetified-games-frame-pacing-1.10.0/prefab/modules/swappy/libs/android.armeabi-v7a_API23_NDK23_cpp_shared_Release/libswappy.so"
    INTERFACE_INCLUDE_DIRECTORIES "/Users/bitfox/.gradle/caches/transforms-3/2f34eafaa5072f0d6f8b0bc1d6fbf165/transformed/jetified-games-frame-pacing-1.10.0/prefab/modules/swappy/include"
    INTERFACE_LINK_LIBRARIES ""
)
endif()

if(NOT TARGET games-frame-pacing::swappy_static)
add_library(games-frame-pacing::swappy_static STATIC IMPORTED)
set_target_properties(games-frame-pacing::swappy_static PROPERTIES
    IMPORTED_LOCATION "/Users/bitfox/.gradle/caches/transforms-3/2f34eafaa5072f0d6f8b0bc1d6fbf165/transformed/jetified-games-frame-pacing-1.10.0/prefab/modules/swappy_static/libs/android.armeabi-v7a_API23_NDK23_cpp_shared_Release/libswappy.a"
    INTERFACE_INCLUDE_DIRECTORIES "/Users/bitfox/.gradle/caches/transforms-3/2f34eafaa5072f0d6f8b0bc1d6fbf165/transformed/jetified-games-frame-pacing-1.10.0/prefab/modules/swappy_static/include"
    INTERFACE_LINK_LIBRARIES ""
)
endif()

