import { ref } from 'vue'
import useAppConfig from '@core/app-config/useAppConfig'
import { useUtils as useI18nUtils } from '@core/libs/i18n'
export default function useAppCustomizer() {
  // Customizer
  const isCustomizerOpen = ref(false)
  const { t } = useI18nUtils()
  // Skin
  const skinOptions = [
    { text: t('Light'), value: 'light' },
    { text: t('Bordered'), value: 'bordered' },
    { text: t('Dark'), value: 'dark' },
    { text: t('Semi Dark'), value: 'semi-dark' },
  ]

  // Content Width Options
  const contentWidthOptions = [
    { text: t('Full Width'), value: 'full' },
    { text: t('Boxed'), value: 'boxed' },
  ]

  // Router Transition
  const routerTransitionOptions = [
    { title: t('Zoom fade'), value: 'zoom-fade' },
    { title: t('Fade'), value: 'fade' },
    { title: t('Fade Bottom'), value: 'fade-bottom' },
    { title: t('Slide Fade'), value: 'slide-fade' },
    { title: t('Zoom out'), value: 'zoom-out' },
    { title: t('None'), value: 'none' },
  ]

  // Router Transition
  const layoutTypeOptions = [
    { text: 'Vertical', value: 'vertical' },
    { text: 'Horizontal', value: 'horizontal' },
  ]

  // Navbar
  const navbarColors = ['', 'primary', 'secondary', 'success', 'danger', 'warning', 'info', 'dark']

  // Navbar Types
  const navbarTypes = [
    { text: t('Floating'), value: 'floating' },
    { text: t('Sticky'), value: 'sticky' },
    { text: t('Static'), value: 'static' },
    { text: t('Hidden'), value: 'hidden' },
  ]

  // Footer Types
  const footerTypes = [
    { text: t('Sticky'), value: 'sticky' },
    { text: t('Static'), value: 'static' },
    { text: t('Hidden'), value: 'hidden' },
  ]

  // eslint-disable-next-line object-curly-newline
  const {
    isRTL,
    skin,
    contentWidth,
    routerTransition,
    layoutType,
    isNavMenuHidden,
    isVerticalMenuCollapsed,
    navbarBackgroundColor,
    navbarType,
    footerType,
  } = useAppConfig()

  return {
    // Customizer
    isCustomizerOpen,

    // Vertical Menu
    isVerticalMenuCollapsed,

    // Skin
    skin,
    skinOptions,

    // Content Width
    contentWidth,
    contentWidthOptions,

    // RTL
    isRTL,

    // routerTransition
    routerTransition,
    routerTransitionOptions,

    // Layout Type
    layoutType,
    layoutTypeOptions,

    // NavMenu Hidden
    isNavMenuHidden,

    // Navbar
    navbarColors,
    navbarTypes,
    navbarBackgroundColor,
    navbarType,

    // Footer
    footerTypes,
    footerType,
  }
}
